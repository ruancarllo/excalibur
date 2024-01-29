using System;
using Rhino;

using System.Windows.Forms;

[assembly: System.Runtime.InteropServices.Guid("8f544372-c3fd-46ee-801b-f6b426b6c0a1")]

namespace Excalibur {
  public class ExcaliburPlugin : Rhino.PlugIns.PlugIn {
    public ExcaliburPlugin() {
      Instance = this;
    }

    public static ExcaliburPlugin Instance { get; private set; }
  }

  public class ExcaliburCommand : Rhino.Commands.Command {
    public ExcaliburCommand() {
      Instance = this;
    }
    
    public static ExcaliburCommand Instance { get; private set; }

    public override System.String EnglishName => "Excalibur";

    protected override Rhino.Commands.Result RunCommand(Rhino.RhinoDoc rhinoDocument, Rhino.Commands.RunMode mode) {
      var excaliburScaler = new ExcaliburScaler(rhinoDocument);
      var excaliburManager = new ExcaliburManager(excaliburScaler);
      var excaliburViewport = new ExcaliburViewport(excaliburManager);

      Rhino.RhinoApp.WriteLine("Hello");
      excaliburViewport.Show();

      return Rhino.Commands.Result.Success;
    }
  }

  public class ExcaliburViewport : Form {
    private readonly ExcaliburManager Manager;
    private readonly TableLayoutPanel FormLayout;

    public ExcaliburViewport(ExcaliburManager excaliburManager) {
      Manager = excaliburManager;
      FormLayout = CreateLayout();

      Text = "Excalibur";
      Padding = new Padding(10);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      TopMost = true;

      Controls.Add(FormLayout);

      Shown += Manager.StartListeningEvents;
      FormClosed += Manager.StopListeningEvents;
    }

    private TableLayoutPanel CreateLayout() {
      var formLayout = new TableLayoutPanel {
          Dock = DockStyle.Fill,
          ColumnCount = 1,
          RowCount = 2,
          Padding = new Padding(5),
          AutoSize = true
      };

      var axisControls = new TableLayoutPanel {
          Dock = DockStyle.Fill,
          ColumnCount = 4,
          RowCount = 3,
          Padding = new Padding(0),
          AutoSize = true
      };

      var footerControls = new TableLayoutPanel {
          Dock = DockStyle.Fill,
          ColumnCount = 5,
          RowCount = 1,
          Padding = new Padding(0),
          AutoSize = true
      };

      formLayout.Controls.Add(axisControls, 0, 0);
      formLayout.Controls.Add(footerControls, 0, 1);

      axisControls.Controls.Add(Manager.CopyXButton, 0, 0);
      axisControls.Controls.Add(Manager.ActualWidthTextBox, 1, 0);
      axisControls.Controls.Add(Manager.XScaleLabel, 2, 0);
      axisControls.Controls.Add(Manager.XFactorTextBox, 3, 0);

      axisControls.Controls.Add(Manager.CopyYButton, 0, 1);
      axisControls.Controls.Add(Manager.ActualHeightTextBox, 1, 1);
      axisControls.Controls.Add(Manager.YScaleLabel, 2, 1);
      axisControls.Controls.Add(Manager.YFactorTextBox, 3, 1);

      axisControls.Controls.Add(Manager.CopyZButton, 0, 2);
      axisControls.Controls.Add(Manager.ActualDepthTextBox, 1, 2);
      axisControls.Controls.Add(Manager.ZScaleLabel, 2, 2);
      axisControls.Controls.Add(Manager.ZFactorTextBox, 3, 2);

      footerControls.Controls.Add(Manager.ContratctionCheckBox, 0, 0);
      footerControls.Controls.Add(Manager.UndoButton, 1, 0);
      footerControls.Controls.Add(Manager.ApplyButton, 2, 0);
      footerControls.Controls.Add(Manager.SubscaleButton, 3, 0);
      footerControls.Controls.Add(Manager.SuperscaleButton, 4, 0);

      return formLayout;
    }
  }

  public class ExcaliburManager {
    private ExcaliburScaler Scaler;
    private ExcaliburState State;

    public Button CopyXButton;
    public Button CopyYButton;
    public Button CopyZButton;

    public TextBox ActualWidthTextBox;
    public TextBox ActualHeightTextBox;
    public TextBox ActualDepthTextBox;

    public Label XScaleLabel;
    public Label YScaleLabel;
    public Label ZScaleLabel;

    public TextBox XFactorTextBox;
    public TextBox YFactorTextBox;
    public TextBox ZFactorTextBox;

    public CheckBox ContratctionCheckBox;

    public Button UndoButton;
    public Button ApplyButton;

    public Button SubscaleButton;
    public Button SuperscaleButton;

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

    private static Button CreateCopyButton() {
      return new Button() {
              Width = 15,
              BackColor = System.Drawing.Color.FromArgb(32, 128, 64)
          };
      }

  private static TextBox CreateActualDimensionsTextBox() {
      return new TextBox() {
          ReadOnly = true
      };
  }

  private static Label CreateScaleLabel(string axisName) {
      return new Label() {
          Text = $"Escala de {axisName}"
      };
  }

  private static TextBox CreateDimensionFactorTextBox() {
      return new TextBox() {
      };
  }

  private static CheckBox CreateContratctionCheckBox() {
      return new CheckBox() {
          Text = "Contração (%)"
      };
  }

  private static Button CreateUndoButton() {
      return new Button() {
          MinimumSize = new System.Drawing.Size(-1, -1),
          Text = "Desfazer"
      };
  }

  private static Button CreateApplyButton() {
      return new Button() {
          MinimumSize = new System.Drawing.Size(-1, -1),
          Text = "Aplicar"
      };
  }

  private static Button CreateSubscaleButton() {
      return new Button {
          MinimumSize = new System.Drawing.Size(-1, -1),
          Text = "-"
      };
  }

  private static Button CreateSuperscaleButton() {
      return new Button {
          MinimumSize = new System.Drawing.Size(-1, -1),
          Text = "+"
      };
  }

  public void StartListeningEvents(object sender, EventArgs e) {
      CopyXButton.Click += HandleCopyXButtonEvent;
      CopyYButton.Click += HandleCopyYButtonEvent;
      CopyZButton.Click += HandleCopyZButtonEvent;

      UndoButton.Click += HandleUndoButtonEvent;
      ApplyButton.Click += HandleApplyButtonEvent;

      SuperscaleButton.Click += HandleSuperscaleButtonEvent;
      SubscaleButton.Click += HandleSubscaleButtonEvent;

      Rhino.RhinoDoc.SelectObjects += HandleSelectionEvent;
  }

  
  public void StopListeningEvents(object sender, EventArgs e) {
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
