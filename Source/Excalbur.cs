namespace Excalibur {
  public class ExcaliburPlugin : Rhino.PlugIns.PlugIn {
    public ExcaliburPlugin() {
      Instance = this;
    }

    public static ExcaliburPlugin Instance {
      get;
      private set;
    }
  }

  public class ExcaliburCommand : Rhino.Commands.Command {
    public ExcaliburCommand() {
      Instance = this;
    }
    
    public static ExcaliburCommand Instance {
      get;
      private set;
    }

    public override string EnglishName => "Excalibur";

    protected override Rhino.Commands.Result RunCommand(Rhino.RhinoDoc rhinoDocument, Rhino.Commands.RunMode mode) {
      var excaliburScaler = new ExcaliburScaler(rhinoDocument);
      var excaliburManager = new ExcaliburManager(excaliburScaler);
      var excaliburViewport = new ExcaliburViewport(excaliburManager);

      excaliburViewport.Show();

      return Rhino.Commands.Result.Success;
    }
  }

  public class ExcaliburViewport : Eto.Forms.Form {
    private readonly ExcaliburManager Manager;
    private readonly Eto.Forms.DynamicLayout Layout;

    public ExcaliburViewport(ExcaliburManager excaliburManager) {
      Manager = excaliburManager;
      Layout = CreateLayout();

      Title = "Excalibur";
      Padding = 10;
      Resizable = false;
      Topmost = true;

      Content = Layout;

      Shown += Manager.StartListeningEvents;
      Closed += Manager.StopListeningEvents;
    }

    private Eto.Forms.DynamicLayout CreateLayout() {
      var formLayout = new Eto.Forms.DynamicLayout();

      var axisControls = new Eto.Forms.DynamicLayout() {
        Spacing = FormSpacing
      };

      var footerControls = new Eto.Forms.DynamicLayout() {
        Spacing = FormSpacing
      };

      formLayout.BeginVertical(spacing: FormSpacing);
      
      axisControls.BeginHorizontal();
      axisControls.Add(Manager.CopyXButton);
      axisControls.Add(Manager.ActualWidthTextBox, true);
      axisControls.Add(Manager.XScaleLabel);
      axisControls.Add(Manager.XFactorTextBox);
      axisControls.EndHorizontal();

      axisControls.BeginHorizontal();
      axisControls.Add(Manager.CopyYButton);
      axisControls.Add(Manager.ActualHeightTextBox, true);
      axisControls.Add(Manager.YScaleLabel);
      axisControls.Add(Manager.YFactorTextBox);
      axisControls.EndHorizontal();

      axisControls.BeginHorizontal();
      axisControls.Add(Manager.CopyZButton);
      axisControls.Add(Manager.ActualDepthTextBox, true);
      axisControls.Add(Manager.ZScaleLabel);
      axisControls.Add(Manager.ZFactorTextBox);
      axisControls.EndHorizontal();

      formLayout.Add(axisControls);

      footerControls.BeginHorizontal();
      footerControls.Add(Manager.ContratctionCheckBox, true);
      footerControls.Add(Manager.UndoButton);
      footerControls.Add(Manager.ApplyButton);
      footerControls.Add(Manager.SubscaleButton);
      footerControls.Add(Manager.SuperscaleButton);
      footerControls.EndHorizontal();

      formLayout.Add(footerControls);

      formLayout.EndVertical();

      return formLayout;
    }

    private static readonly Eto.Drawing.Size FormSpacing = new Eto.Drawing.Size(5, 5);
  }

  public class ExcaliburManager {
    private ExcaliburScaler Scaler;
    private ExcaliburState State;

    public Eto.Forms.Button CopyXButton;
    public Eto.Forms.Button CopyYButton;
    public Eto.Forms.Button CopyZButton;

    public Eto.Forms.TextBox ActualWidthTextBox;
    public Eto.Forms.TextBox ActualHeightTextBox;
    public Eto.Forms.TextBox ActualDepthTextBox;

    public Eto.Forms.Label XScaleLabel;
    public Eto.Forms.Label YScaleLabel;
    public Eto.Forms.Label ZScaleLabel;

    public Eto.Forms.TextBox XFactorTextBox;
    public Eto.Forms.TextBox YFactorTextBox;
    public Eto.Forms.TextBox ZFactorTextBox;

    public Eto.Forms.CheckBox ContratctionCheckBox;

    public Eto.Forms.Button UndoButton;
    public Eto.Forms.Button ApplyButton;

    public Eto.Forms.Button SubscaleButton;
    public Eto.Forms.Button SuperscaleButton;

    public ExcaliburManager(ExcaliburScaler excaliburScaler) {
      Scaler = excaliburScaler;
      State = new ExcaliburState();

      CreateForms();
      SetActualDimensionsTextBoxes();
    }

    private void CreateForms() {
      CopyXButton = CreateCopyButton();
      CopyYButton = CreateCopyButton();
      CopyZButton = CreateCopyButton();

      ActualWidthTextBox = CreateActualDimensionsTextBox();
      ActualHeightTextBox = CreateActualDimensionsTextBox();
      ActualDepthTextBox = CreateActualDimensionsTextBox();

      XScaleLabel = CreateScaleLabel("X");
      YScaleLabel = CreateScaleLabel("Y");
      ZScaleLabel = CreateScaleLabel("Z");

      XFactorTextBox = CreateDimensionFactorTextBox();
      YFactorTextBox = CreateDimensionFactorTextBox();
      ZFactorTextBox = CreateDimensionFactorTextBox();

      ContratctionCheckBox = CreateContratctionCheckBox();

      UndoButton = CreateUndoButton();
      ApplyButton = CreateApplyButton();

      SubscaleButton = CreateSubscaleButton();
      SuperscaleButton = CreateSuperscaleButton();
    }

    private static Eto.Forms.Button CreateCopyButton() {
      return new Eto.Forms.Button() {
        Width = 15,
        BackgroundColor = Eto.Drawing.Color.FromArgb(32, 128, 64)
      };
    }

    private static Eto.Forms.TextBox CreateActualDimensionsTextBox() {
      return new Eto.Forms.TextBox() {
        ReadOnly = true,
        PlaceholderText = "0.0"
      };
    }

    private static Eto.Forms.Label CreateScaleLabel(string axisName) {
      return new Eto.Forms.Label() {
        Text = $"Escala de {axisName}",
        TextAlignment = Eto.Forms.TextAlignment.Center
      };
    }

    private static Eto.Forms.TextBox CreateDimensionFactorTextBox() {
      return new Eto.Forms.TextBox() {
        PlaceholderText = "0.0",
      };
    }

    private static Eto.Forms.CheckBox CreateContratctionCheckBox() {
      return new Eto.Forms.CheckBox() {
        Text = "Contração (%)"
      }; 
    }

    private static Eto.Forms.Button CreateUndoButton() {
      return new Eto.Forms.Button() {
        MinimumSize = new Eto.Drawing.Size(-1, -1),
        Text = "Desfazer"
      };
    }

    private static Eto.Forms.Button CreateApplyButton() {
      return new Eto.Forms.Button() {
        MinimumSize = new Eto.Drawing.Size(-1, -1),
        Text = "Aplicar"
      };
    }

    private static Eto.Forms.Button CreateSubscaleButton() {
      return new Eto.Forms.Button {
        MinimumSize = new Eto.Drawing.Size(-1, -1),
        Text = "-"
      };
    }

    private static Eto.Forms.Button CreateSuperscaleButton() {
      return new Eto.Forms.Button {
        MinimumSize = new Eto.Drawing.Size(-1, -1),
        Text = "+"
      };
    }

    public void StartListeningEvents(object sender, System.EventArgs e) {
      CopyXButton.Click += HandleCopyXButtonEvent;
      CopyYButton.Click += HandleCopyYButtonEvent;
      CopyZButton.Click += HandleCopyZButtonEvent;

      UndoButton.Click += HandleUndoButtonEvent;
      ApplyButton.Click += HandleApplyButtonEvent;

      SuperscaleButton.Click += HandleSuperscaleButtonEvent;
      SubscaleButton.Click += HandleSubscaleButtonEvent;

      Rhino.RhinoDoc.SelectObjects += HandleSelectionEvent;
    }

    public void StopListeningEvents(object sender, System.EventArgs e) {
      CopyXButton.Click -= HandleCopyXButtonEvent;
      CopyYButton.Click -= HandleCopyYButtonEvent;
      CopyZButton.Click -= HandleCopyZButtonEvent;

      UndoButton.Click -= HandleUndoButtonEvent;
      ApplyButton.Click -= HandleApplyButtonEvent;

      SuperscaleButton.Click -= HandleSuperscaleButtonEvent;
      SubscaleButton.Click -= HandleSubscaleButtonEvent;

      Rhino.RhinoDoc.SelectObjects -= HandleSelectionEvent;
    } 
    
    private void HandleSelectionEvent(object sender, Rhino.DocObjects.RhinoObjectSelectionEventArgs e) {
      if (Scaler.HasJustCommitedChanges) {
        Scaler.HasJustCommitedChanges = false;
      } else {
        State = new ExcaliburState();
      }

      Scaler = Scaler.Reinstantiate();
      SetActualDimensionsTextBoxes();
    }

    private void HandleCopyXButtonEvent(object sender, System.EventArgs e) {
      XFactorTextBox.Text = ActualWidthTextBox.Text;
    }

    private void HandleCopyYButtonEvent(object sender, System.EventArgs e) {
      YFactorTextBox.Text = ActualHeightTextBox.Text;
    }

    private void HandleCopyZButtonEvent(object sender, System.EventArgs e) {
      ZFactorTextBox.Text = ActualDepthTextBox.Text;
    }

    private void HandleUndoButtonEvent(object sender, System.EventArgs e) {
      if (!State.AreDimensionsHistoryEmpty) {
        Scaler.ScaleAnalyzedObjectsByMode(
          ExcaliburScaler.ScalingModes.Definition,
          ExcaliburScaler.ScalingDirections.None,
          State.LastWidth,
          State.LastHeight,
          State.LastDepth
        );

        State.RemoveLastDimensions();
      }
    }

    private void HandleApplyButtonEvent(object sender, System.EventArgs e) {
      State.AddLastDimensions(Scaler.SelectedObjectsActualWidth, Scaler.SelectedObjectsActualHeight, Scaler.SelectedObjectsActualDepth);

      if (ContratctionCheckBox.Checked == false) {
        if (!double.TryParse(XFactorTextBox.Text, out double widthDefinition)) widthDefinition = Scaler.SelectedObjectsActualWidth;
        if (!double.TryParse(YFactorTextBox.Text, out double heightDefinition)) heightDefinition = Scaler.SelectedObjectsActualHeight;
        if (!double.TryParse(ZFactorTextBox.Text, out double depthDefinition)) depthDefinition = Scaler.SelectedObjectsActualDepth;

        Scaler.ScaleAnalyzedObjectsByMode(
          ExcaliburScaler.ScalingModes.Definition,
          ExcaliburScaler.ScalingDirections.None,
          widthDefinition,
          heightDefinition,
          depthDefinition
        );
      }

      if (ContratctionCheckBox.Checked == true) {
        if (!double.TryParse(XFactorTextBox.Text, out double widthContraction)) widthContraction = 0;
        if (!double.TryParse(YFactorTextBox.Text, out double heightContraction)) heightContraction = 0;
        if (!double.TryParse(ZFactorTextBox.Text, out double depthContraction)) depthContraction = 0;

        Scaler.ScaleAnalyzedObjectsByMode(
          ExcaliburScaler.ScalingModes.Contraction,
          ExcaliburScaler.ScalingDirections.None,
          widthContraction,
          heightContraction,
          depthContraction
        );
      }
    }

    private void HandleSuperscaleButtonEvent(object sender, System.EventArgs e) {
      if (!double.TryParse(XFactorTextBox.Text, out double widthProgression)) widthProgression = 0;
      if (!double.TryParse(YFactorTextBox.Text, out double heightProgression)) heightProgression = 0;
      if (!double.TryParse(ZFactorTextBox.Text, out double depthProgression)) depthProgression = 0;

      Scaler.ScaleAnalyzedObjectsByMode(
        ExcaliburScaler.ScalingModes.Progression,
        ExcaliburScaler.ScalingDirections.Increasing,
        widthProgression,
        heightProgression,
        depthProgression
      );
    }

    private void HandleSubscaleButtonEvent(object sender, System.EventArgs e) {
      if (!double.TryParse(XFactorTextBox.Text, out double widthProgression)) widthProgression = 0;
      if (!double.TryParse(YFactorTextBox.Text, out double heightProgression)) heightProgression = 0;
      if (!double.TryParse(ZFactorTextBox.Text, out double depthProgression)) depthProgression = 0;

      Scaler.ScaleAnalyzedObjectsByMode(
        ExcaliburScaler.ScalingModes.Progression,
        ExcaliburScaler.ScalingDirections.Decreasing,
        widthProgression,
        heightProgression,
        depthProgression
      );
    }

    private void SetActualDimensionsTextBoxes() {
      ActualWidthTextBox.Text = Scaler.SelectedObjectsActualWidth.ToString();
      ActualHeightTextBox.Text = Scaler.SelectedObjectsActualHeight.ToString();
      ActualDepthTextBox.Text = Scaler.SelectedObjectsActualDepth.ToString();
    }
  }

  public class ExcaliburScaler {
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
    
    public ExcaliburScaler(Rhino.RhinoDoc rhinoDocument) {
      RhinoDocument = rhinoDocument;

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

      AnalyzedObjectsInitialXCenter = analyzedObjectsCombinedBoundingBoxes.Center.X;
      AnalyzedObjectsInitialYCenter = analyzedObjectsCombinedBoundingBoxes.Center.Y;
      AnalyzedObjectsInitialZCenter = analyzedObjectsCombinedBoundingBoxes.Center.Z;

      AnalyzedObjects = analyzedObjectsList.ToArray();

      ProgressionIncreasingIndex = 0;
      ProgressionDecreasingIndex = 0;

      HasProgressionFactorsBeenSet = false;
      HasJustCommitedChanges = true;
    }

    public void ScaleAnalyzedObjectsByMode( ScalingModes scalingMode, ScalingDirections scalingDirection, double xRawFactor, double yRawFactor, double zRawFactor) {
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

      double xScaleFactor = 1;
      double yScaleFactor = 1;
      double zScaleFactor = 1;

      int progressionIndex = 0;
      int scalingTernary = (int)scalingDirection;

      string progressionDirection = "";

      var scalingTransformation = Rhino.Geometry.Transform.Identity;
      var translationTransformation = Rhino.Geometry.Transform.Identity;

      if (scalingMode == ScalingModes.Definition) {
        xScaleFactor = xRawFactor / SelectedObjectsActualWidth;
        yScaleFactor = yRawFactor / SelectedObjectsActualHeight;
        zScaleFactor = zRawFactor / SelectedObjectsActualDepth;
      }

      if (scalingMode == ScalingModes.Contraction) {
        xScaleFactor = (xRawFactor + 100) / 100;
        yScaleFactor = (yRawFactor + 100) / 100;
        zScaleFactor = (zRawFactor + 100) / 100;
      }

      if (scalingMode == ScalingModes.Progression) {
        if (scalingDirection == ScalingDirections.Increasing) {
          progressionIndex = ++ProgressionIncreasingIndex;
          progressionDirection = "Increasing";
        }

        if (scalingDirection == ScalingDirections.Decreasing) {
          progressionIndex = ++ProgressionDecreasingIndex;
          progressionDirection = "Decreasing";
        }

        double selectedObjectsNewWidth = SelectedObjectsInitialWidth + ProgressionInitialXFactor * progressionIndex * scalingTernary;
        double selectedObjectsNewHeight = SelectedObjectsInitialHeight + ProgressionInitialYFactor * progressionIndex * scalingTernary;
        double selectedObjectsNewDepth = SelectedObjectsInitialDepth + ProgressionInitialZFactor * progressionIndex * scalingTernary;

        xScaleFactor = selectedObjectsNewWidth / SelectedObjectsInitialWidth;
        yScaleFactor = selectedObjectsNewHeight / SelectedObjectsInitialHeight;
        zScaleFactor = selectedObjectsNewDepth / SelectedObjectsInitialDepth;

        double progressionTriangularYTranslation = ProgressionInitialYFactor * (progressionIndex * progressionIndex + progressionIndex) / 2;
        double progressionRegularYTranslation = (AnalyzedObjectsInitialHeight + IncreasingSpacing) * progressionIndex * scalingTernary;

        translationTransformation.M13 += progressionTriangularYTranslation + progressionRegularYTranslation;
      }

      translationTransformation.M03 += -xScaleFactor * AnalyzedObjectsInitialXCenter + AnalyzedObjectsInitialXCenter;
      translationTransformation.M13 += -yScaleFactor * AnalyzedObjectsInitialYCenter + AnalyzedObjectsInitialYCenter;
      translationTransformation.M23 += -zScaleFactor * AnalyzedObjectsInitialZCenter + AnalyzedObjectsInitialZCenter;

      scalingTransformation.M00 = xScaleFactor;
      scalingTransformation.M11 = yScaleFactor;
      scalingTransformation.M22 = zScaleFactor;

      if (xScaleFactor <= 0) return;
      if (yScaleFactor <= 0) return;
      if (zScaleFactor <= 0) return;

      if (scalingMode == ScalingModes.Definition || scalingMode == ScalingModes.Contraction) {
        HasJustCommitedChanges = true;

        foreach (var analyzedObject in AnalyzedObjects) {
          analyzedObject.Geometry.Transform(scalingTransformation);
          analyzedObject.CommitChanges();
        }

        SelectedObjectsActualWidth = xScaleFactor * SelectedObjectsActualWidth;
        SelectedObjectsActualHeight = yScaleFactor * SelectedObjectsActualHeight;
        SelectedObjectsActualDepth = zScaleFactor * SelectedObjectsActualDepth;
      }

      if (scalingMode == ScalingModes.Progression) {
        foreach (var analyzedObject in AnalyzedObjects) {
          var analyzedObjectGeometryCopy = analyzedObject.Geometry.Duplicate();
          var analyzedObjectAttributesCopy = analyzedObject.Attributes.Duplicate();

          var analyzedObjectNewLayerCount = RhinoDocument.Layers.Count + 1;

          var analyzedObjectNewLayer = new Rhino.DocObjects.Layer {
            Name = $"Excalibur ${progressionDirection} Layer {analyzedObjectNewLayerCount.ToString()}-{progressionIndex.ToString()}",
            Color = GenerateRandomColor(),
            IsVisible = true,
            IsLocked = false
          };

          var analyzedObjectNewLayerIndex = RhinoDocument.Layers.Add(analyzedObjectNewLayer);

          analyzedObjectAttributesCopy.RemoveFromAllGroups();
          analyzedObjectAttributesCopy.LayerIndex = analyzedObjectNewLayerIndex;

          analyzedObjectGeometryCopy.Transform(scalingTransformation);
          analyzedObjectGeometryCopy.Transform(translationTransformation);

          RhinoDocument.Objects.Add(analyzedObjectGeometryCopy, analyzedObjectAttributesCopy);
        }
      }

      RhinoDocument.Views.Redraw();
    }

    private System.Drawing.Color GenerateRandomColor() {
      var random = new System.Random();

      var red = random.Next(256);
      var green = random.Next(256);
      var blue = random.Next(256);

      return System.Drawing.Color.FromArgb(red, green, blue);
    }

    public ExcaliburScaler Reinstantiate() {
      return new ExcaliburScaler(RhinoDocument);
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