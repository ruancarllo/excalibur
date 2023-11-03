namespace ExcaliburMetrics {
  public class ExcaliburDilator {
    private readonly Rhino.RhinoDoc RhinoDocument;
    private readonly Rhino.DocObjects.RhinoObject[] AnalyzedObjects;

    public double SelectedObjectsActualWidth;
    public double SelectedObjectsActualHeight;
    public double SelectedObjectsActualDepth;

    private readonly double SelectedObjectsInitialWidth;
    private readonly double SelectedObjectsInitialHeight;
    private readonly double SelectedObjectsInitialDepth;

    private readonly double AnalyzedObjectsInitialWidth;
    private readonly double AnalyzedObjectsInitialHeight;
    private readonly double AnalyzedObjectsInitialDepth;

    private readonly double AnalyzedObjectsInitialXCenter;
    private readonly double AnalyzedObjectsInitialYCenter;
    private readonly double AnalyzedObjectsInitialZCenter;

    private double ProgressionInitialXFactor;
    private double ProgressionInitialYFactor;
    private double ProgressionInitialZFactor;

    private int ProgressionIncreasingIndex;
    private int ProgressionDecreasingIndex;

    private bool HasProgressionFactorsBeenSet;
    public bool HasJustCommitedChanges;

    private static readonly double IncreasingSpacing = 5;

    public enum ScalingModes { Definition, Contraction, Progression }
    public enum ScalingDirections { Decreasing = -1, None = 0, Increasing = +1 }
    
    public ExcaliburDilator() {
      RhinoDocument = Rhino.RhinoDoc.ActiveDoc;

      var selectedObjects = RhinoDocument.Objects.GetSelectedObjects(false, false);
      var selectedObjectsList = new System.Collections.Generic.List<Rhino.DocObjects.RhinoObject>(selectedObjects);

      var selectedObjectsGuidsList = new System.Collections.Generic.List<System.Guid>();
      var colateralObjectsGuidsList = new System.Collections.Generic.List<System.Guid>();

      foreach (var selectedObject in selectedObjectsList) {
        selectedObjectsGuidsList.Add(selectedObject.Id);
      }

      foreach (var selectedObject in selectedObjectsList) {
        var selectedObjectGroupIndexList = selectedObject.GetGroupList();

        if (selectedObjectGroupIndexList != null) {
          foreach (var selectedObjectGroupIndex in selectedObjectGroupIndexList) {
            var selectedObjectGroupObjects = RhinoDocument.Objects.FindByGroup(selectedObjectGroupIndex);

            foreach (var selectedObjectGroupObject in selectedObjectGroupObjects) {
              if (!selectedObjectsGuidsList.Contains(selectedObjectGroupObject.Id)) {
                colateralObjectsGuidsList.Add(selectedObjectGroupObject.Id);
              }
            }
          }
        }
      }

      var analyzedObjectsList = new System.Collections.Generic.List<Rhino.DocObjects.RhinoObject>();

      foreach (var selectedObjectGuid in selectedObjectsGuidsList) {
        var selectedObject = RhinoDocument.Objects.Find(selectedObjectGuid);
        analyzedObjectsList.Add(selectedObject);
      }

      foreach (var colateralObjectGuid in colateralObjectsGuidsList) {
        var colateralObject = RhinoDocument.Objects.Find(colateralObjectGuid);
        analyzedObjectsList.Add(colateralObject);
      }

      var selectedObjectsCombinedBoundingBoxes = Rhino.Geometry.BoundingBox.Empty;

      foreach (var selectedObject in selectedObjectsList) {
        var selectedObjectBoungingBox = selectedObject.Geometry.GetBoundingBox(true);
        selectedObjectsCombinedBoundingBoxes.Union(selectedObjectBoungingBox);
      }

      SelectedObjectsActualWidth = selectedObjectsCombinedBoundingBoxes.Max.X - selectedObjectsCombinedBoundingBoxes.Min.X;
      SelectedObjectsActualHeight = selectedObjectsCombinedBoundingBoxes.Max.Y - selectedObjectsCombinedBoundingBoxes.Min.Y;
      SelectedObjectsActualDepth = selectedObjectsCombinedBoundingBoxes.Max.Z - selectedObjectsCombinedBoundingBoxes.Min.Z;

      if (SelectedObjectsActualWidth < 0) SelectedObjectsActualWidth = 0;
      if (SelectedObjectsActualHeight < 0) SelectedObjectsActualHeight = 0;
      if (SelectedObjectsActualDepth < 0) SelectedObjectsActualDepth = 0;

      SelectedObjectsInitialWidth = SelectedObjectsActualWidth;
      SelectedObjectsInitialHeight = SelectedObjectsActualHeight;
      SelectedObjectsInitialDepth = SelectedObjectsActualDepth;

      var analyzedObjectsCombinedBoundingBoxes = Rhino.Geometry.BoundingBox.Empty;

      foreach (var analyzedObject in analyzedObjectsList) {
        var analyzedObjectBoungingBox = analyzedObject.Geometry.GetBoundingBox(true);
        analyzedObjectsCombinedBoundingBoxes.Union(analyzedObjectBoungingBox);
      }

      AnalyzedObjectsInitialWidth = analyzedObjectsCombinedBoundingBoxes.Max.X - analyzedObjectsCombinedBoundingBoxes.Min.X;
      AnalyzedObjectsInitialHeight = analyzedObjectsCombinedBoundingBoxes.Max.Y - analyzedObjectsCombinedBoundingBoxes.Min.Y;
      AnalyzedObjectsInitialDepth = analyzedObjectsCombinedBoundingBoxes.Max.Z - analyzedObjectsCombinedBoundingBoxes.Min.Z;

      if (AnalyzedObjectsInitialWidth < 0) AnalyzedObjectsInitialWidth = 0;
      if (AnalyzedObjectsInitialHeight < 0) AnalyzedObjectsInitialHeight = 0;
      if (AnalyzedObjectsInitialDepth < 0) AnalyzedObjectsInitialDepth = 0;

      AnalyzedObjectsInitialXCenter = analyzedObjectsCombinedBoundingBoxes.Center.X;
      AnalyzedObjectsInitialYCenter = analyzedObjectsCombinedBoundingBoxes.Center.Y;
      AnalyzedObjectsInitialZCenter = analyzedObjectsCombinedBoundingBoxes.Center.Z;

      AnalyzedObjects = analyzedObjectsList.ToArray();

      ProgressionIncreasingIndex = 0;
      ProgressionDecreasingIndex = 0;

      HasProgressionFactorsBeenSet = false;
      HasJustCommitedChanges = true;
    }

    public void DilateAnalyzedObjectsByMode(ScalingModes scalingMode, ScalingDirections scalingDirection, double xRawFactor, double yRawFactor, double zRawFactor) {
      if (!HasProgressionFactorsBeenSet) {
        ProgressionInitialXFactor = xRawFactor;
        ProgressionInitialYFactor = yRawFactor;
        ProgressionInitialZFactor = zRawFactor;

        HasProgressionFactorsBeenSet = true;
      } else {
        if (ProgressionInitialXFactor != xRawFactor || ProgressionInitialYFactor != yRawFactor || ProgressionInitialZFactor != zRawFactor) {
          Rhino.RhinoApp.WriteLine("Não altere o fator de progressão durante uma escala!", "Alerta do Excalibur");
          return;
        }
      }

      double xDilateFactor = 1;
      double yDilateFactor = 1;
      double zDilateFactor = 1;

      int progressionIndex = 0;
      int scalingTernary = (int)scalingDirection;

      var scalingTransformation = Rhino.Geometry.Transform.Identity;
      var translationTransformation = Rhino.Geometry.Transform.Identity;

      if (scalingMode == ScalingModes.Definition) {
        xDilateFactor = xRawFactor / SelectedObjectsActualWidth;
        yDilateFactor = yRawFactor / SelectedObjectsActualHeight;
        zDilateFactor = zRawFactor / SelectedObjectsActualDepth;
      }

      if (scalingMode == ScalingModes.Contraction) {
        xDilateFactor = (xRawFactor + 100) / 100;
        yDilateFactor = (yRawFactor + 100) / 100;
        zDilateFactor = (zRawFactor + 100) / 100;
      }

      if (scalingMode == ScalingModes.Progression) {
        if (scalingDirection == ScalingDirections.Increasing) {
          progressionIndex = ++ProgressionIncreasingIndex;
        }

        if (scalingDirection == ScalingDirections.Decreasing) {
          progressionIndex = ++ProgressionDecreasingIndex;
        }

        double selectedObjectsNewWidth = SelectedObjectsInitialWidth + ProgressionInitialXFactor * progressionIndex * scalingTernary;
        double selectedObjectsNewHeight = SelectedObjectsInitialHeight + ProgressionInitialYFactor * progressionIndex * scalingTernary;
        double selectedObjectsNewDepth = SelectedObjectsInitialDepth + ProgressionInitialZFactor * progressionIndex * scalingTernary;

        xDilateFactor = selectedObjectsNewWidth / SelectedObjectsInitialWidth;
        yDilateFactor = selectedObjectsNewHeight / SelectedObjectsInitialHeight;
        zDilateFactor = selectedObjectsNewDepth / SelectedObjectsInitialDepth;

        double progressionTriangularYTranslation = ProgressionInitialYFactor * (progressionIndex * progressionIndex + progressionIndex) / 2;
        double progressionRegularYTranslation = (AnalyzedObjectsInitialHeight + IncreasingSpacing) * progressionIndex * scalingTernary;

        translationTransformation.M13 += progressionTriangularYTranslation + progressionRegularYTranslation;
      }

      translationTransformation.M03 += -xDilateFactor * AnalyzedObjectsInitialXCenter + AnalyzedObjectsInitialXCenter;
      translationTransformation.M13 += -yDilateFactor * AnalyzedObjectsInitialYCenter + AnalyzedObjectsInitialYCenter;
      translationTransformation.M23 += -zDilateFactor * AnalyzedObjectsInitialZCenter + AnalyzedObjectsInitialZCenter;

      scalingTransformation.M00 = xDilateFactor;
      scalingTransformation.M11 = yDilateFactor;
      scalingTransformation.M22 = zDilateFactor;

      if (xDilateFactor <= 0) return;
      if (yDilateFactor <= 0) return;
      if (zDilateFactor <= 0) return;

      if (scalingMode == ScalingModes.Definition || scalingMode == ScalingModes.Contraction) {
        HasJustCommitedChanges = true;

        foreach (var analyzedObject in AnalyzedObjects) {
          analyzedObject.Geometry.Transform(scalingTransformation);
          analyzedObject.CommitChanges();
        }

        SelectedObjectsActualWidth = xDilateFactor * SelectedObjectsActualWidth;
        SelectedObjectsActualHeight = yDilateFactor * SelectedObjectsActualHeight;
        SelectedObjectsActualDepth = zDilateFactor * SelectedObjectsActualDepth;
      }

      if (scalingMode == ScalingModes.Progression) {
        var dilatedObjectsIds = new System.Collections.Generic.List<System.Guid>();

        foreach (var analyzedObject in AnalyzedObjects) {
          if (!dilatedObjectsIds.Contains(analyzedObject.Id)) {
            var analyzedObjectGeometryCopy = analyzedObject.Geometry.Duplicate();
            var analyzedObjectAttributesCopy = analyzedObject.Attributes.Duplicate();

            analyzedObjectAttributesCopy.RemoveFromAllGroups();

            analyzedObjectGeometryCopy.Transform(scalingTransformation);
            analyzedObjectGeometryCopy.Transform(translationTransformation);

            RhinoDocument.Objects.Add(analyzedObjectGeometryCopy, analyzedObjectAttributesCopy);

            dilatedObjectsIds.Add(analyzedObject.Id);
          }          
        }
      }

      RhinoDocument.Views.Redraw();
    }

    public ExcaliburDilator Reinstantiate() {
      return new ExcaliburDilator();
    }
  }

  public class ExcaliburState {
    private readonly System.Collections.Generic.List<double> WidthHistoryList;
    private readonly System.Collections.Generic.List<double> HeightHistoryList;
    private readonly System.Collections.Generic.List<double> DepthHistoryList;

    public double LastWidth => WidthHistoryList[WidthHistoryList.Count - 1];
    public double LastHeight => HeightHistoryList[HeightHistoryList.Count - 1];
    public double LastDepth => DepthHistoryList[DepthHistoryList.Count - 1];

    public bool AreDimensionsHistoryEmpty => WidthHistoryList.Count == 0 && HeightHistoryList.Count == 0 && DepthHistoryList.Count == 0;

    public ExcaliburState() {
      WidthHistoryList = new System.Collections.Generic.List<double>();
      HeightHistoryList = new System.Collections.Generic.List<double>();
      DepthHistoryList = new System.Collections.Generic.List<double>();
    }

    public void AddLastDimensions(double width, double height, double depth) {
      WidthHistoryList.Add(width);
      HeightHistoryList.Add(height);
      DepthHistoryList.Add(depth);
    }

    public void RemoveLastDimensions() {
      WidthHistoryList.RemoveAt(WidthHistoryList.Count - 1);
      HeightHistoryList.RemoveAt(HeightHistoryList.Count - 1);
      DepthHistoryList.RemoveAt(DepthHistoryList.Count - 1);
    }
  }
}