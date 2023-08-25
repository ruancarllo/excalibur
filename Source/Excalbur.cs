using System;
using Rhino;
using Eto;

namespace Excalibur;

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

  private static readonly Eto.Drawing.Size FormSpacing = new(5, 5);
}

public class ExcaliburManager {
  public ExcaliburScaler Scaler;
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
    State = new ExcaliburState(Scaler.RhinoDocument);

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
      Text = "Refazer"
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
    if (!Scaler.JustScaled) {
      // A pure selection event has occurred
      State = new ExcaliburState(Scaler.RhinoDocument);
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
    Scaler.DeleteSelectedObjects();
    State.RestoreInitialObjects();
  }

  private void HandleApplyButtonEvent(object sender, System.EventArgs e) {
    SetInitialStateIfUnset();

    if ((bool)ContratctionCheckBox.Checked == false) {
      if (!double.TryParse(XFactorTextBox.Text, out double widthDefinition)) widthDefinition = Scaler.SelectedObjectsActualWidth;
      if (!double.TryParse(YFactorTextBox.Text, out double heightDefinition)) heightDefinition = Scaler.SelectedObjectsActualHeight;
      if (!double.TryParse(ZFactorTextBox.Text, out double depthDefinition)) depthDefinition = Scaler.SelectedObjectsActualDepth;

      Scaler.ScaleSelectedObjectsByMode(ExcaliburScaler.ScalingModes.Definition, ExcaliburScaler.ScalingDirections.None, widthDefinition, heightDefinition, depthDefinition);
    }

    if ((bool)ContratctionCheckBox.Checked == true) {
      if (!double.TryParse(XFactorTextBox.Text, out double widthContraction)) widthContraction = 0;
      if (!double.TryParse(YFactorTextBox.Text, out double heightContraction)) heightContraction = 0;
      if (!double.TryParse(ZFactorTextBox.Text, out double depthContraction)) depthContraction = 0;

      Scaler.ScaleSelectedObjectsByMode(ExcaliburScaler.ScalingModes.Contraction, ExcaliburScaler.ScalingDirections.None, widthContraction, heightContraction, depthContraction);
    }

    SetActualDimensionsTextBoxes();
  }

  private void HandleSuperscaleButtonEvent(object sender, System.EventArgs e) {
    if (!double.TryParse(XFactorTextBox.Text, out double widthProgression)) widthProgression = 0;
    if (!double.TryParse(YFactorTextBox.Text, out double heightProgression)) heightProgression = 0;
    if (!double.TryParse(ZFactorTextBox.Text, out double depthProgression)) depthProgression = 0;

    Scaler.ScaleSelectedObjectsByMode(ExcaliburScaler.ScalingModes.Progression, ExcaliburScaler.ScalingDirections.Increasing, widthProgression, heightProgression, depthProgression);
  }

  private void HandleSubscaleButtonEvent(object sender, System.EventArgs e) {
    if (!double.TryParse(XFactorTextBox.Text, out double widthProgression)) widthProgression = 0;
    if (!double.TryParse(YFactorTextBox.Text, out double heightProgression)) heightProgression = 0;
    if (!double.TryParse(ZFactorTextBox.Text, out double depthProgression)) depthProgression = 0;

    Scaler.ScaleSelectedObjectsByMode(ExcaliburScaler.ScalingModes.Progression, ExcaliburScaler.ScalingDirections.Decreasing, widthProgression, heightProgression, depthProgression);
  }

  private void SetActualDimensionsTextBoxes() {
    ActualWidthTextBox.Text = Scaler.SelectedObjectsActualWidth.ToString();
    ActualHeightTextBox.Text = Scaler.SelectedObjectsActualHeight.ToString();
    ActualDepthTextBox.Text = Scaler.SelectedObjectsActualDepth.ToString();
  }
  
  private void SetInitialStateIfUnset() {
    if (!State.AreObjectsSet) {
      State.SetInitialObjects(Scaler.SelectedObjects);
    }
  }
}

public class ExcaliburScaler {
  public readonly Rhino.RhinoDoc RhinoDocument;
  public readonly Rhino.DocObjects.RhinoObject[] SelectedObjects;

  public bool AreThereSelectedObjects => SelectedObjects.Length > 0;

  public double SelectedObjectsActualWidth;
  public double SelectedObjectsActualHeight;
  public double SelectedObjectsActualDepth;

  private readonly double ScaledObjectsInitialWidth;
  private readonly double ScaledObjectsInitialHeight;
  private readonly double ScaledObjectsInitialDepth;

  private readonly double ScaledObjectsInitialXCenter;
  private readonly double ScaledObjectsInitialYCenter;
  private readonly double ScaledObjectsInitialZCenter;

  private double ProgressionInitialXFactor;
  private double ProgressionInitialYFactor;
  private double ProgressionInitialZFactor;

  private int ProgressionIncreasingIndex;
  private int ProgressionDecreasingIndex;

  private bool HasProgressionFactorsBeenSet;
  private bool HasScaledByDefinitionOrContraction;

  public bool JustScaled;

  private static readonly double IncreasingSpacing = 5;

  public enum ScalingModes { Definition, Contraction, Progression }
  public enum ScalingDirections { Decreasing = -1, None = 0, Increasing = +1 }
  
  public ExcaliburScaler(Rhino.RhinoDoc rhinoDocument) {
    RhinoDocument = rhinoDocument;

    var objects = RhinoDocument.Objects.GetSelectedObjects(false, false);
    var objectsList = new System.Collections.Generic.List<Rhino.DocObjects.RhinoObject>(objects);
    var objestsArray = objectsList.ToArray();

    SelectedObjects = objestsArray;

    var selectedObjectsCombinedBoundingBoxes = Rhino.Geometry.BoundingBox.Empty;

    foreach (var rhinoObject in SelectedObjects) {
      var rhinoObjectBoungingBox = rhinoObject.Geometry.GetBoundingBox(true);
      selectedObjectsCombinedBoundingBoxes.Union(rhinoObjectBoungingBox);
    }

    SelectedObjectsActualWidth = selectedObjectsCombinedBoundingBoxes.Max.X - selectedObjectsCombinedBoundingBoxes.Min.X;
    SelectedObjectsActualHeight = selectedObjectsCombinedBoundingBoxes.Max.Y - selectedObjectsCombinedBoundingBoxes.Min.Y;
    SelectedObjectsActualDepth = selectedObjectsCombinedBoundingBoxes.Max.Z - selectedObjectsCombinedBoundingBoxes.Min.Z;

    ScaledObjectsInitialWidth = SelectedObjectsActualWidth;
    ScaledObjectsInitialHeight = SelectedObjectsActualHeight;
    ScaledObjectsInitialDepth = SelectedObjectsActualDepth;

    ScaledObjectsInitialXCenter = selectedObjectsCombinedBoundingBoxes.Center.X;
    ScaledObjectsInitialYCenter = selectedObjectsCombinedBoundingBoxes.Center.Y;
    ScaledObjectsInitialZCenter = selectedObjectsCombinedBoundingBoxes.Center.Z;

    ProgressionIncreasingIndex = 0;
    ProgressionDecreasingIndex = 0;

    HasProgressionFactorsBeenSet = false;
    HasScaledByDefinitionOrContraction = false;

    JustScaled = false;
  }

  public void ScaleSelectedObjectsByMode(ScalingModes scalingMode, ScalingDirections scalingDirection, double xRawFactor, double yRawFactor, double zRawFactor) {
    JustScaled = true;
   
    if (!HasProgressionFactorsBeenSet) {
      ProgressionInitialXFactor = xRawFactor;
      ProgressionInitialYFactor = yRawFactor;
      ProgressionInitialZFactor = zRawFactor;

      HasProgressionFactorsBeenSet = true;
    }

    double xScaleFactor = 1;
    double yScaleFactor = 1;
    double zScaleFactor = 1;

    int progressionIndex = 0;
    int scalingTernary = (int)scalingDirection;

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
      if (scalingDirection == ScalingDirections.Increasing) progressionIndex = ++ProgressionIncreasingIndex;
      if (scalingDirection == ScalingDirections.Decreasing) progressionIndex = ++ProgressionDecreasingIndex;

      double selectedObjectsNewWidth = ScaledObjectsInitialWidth + ProgressionInitialXFactor * progressionIndex * scalingTernary;
      double selectedObjectsNewHeight = ScaledObjectsInitialHeight + ProgressionInitialYFactor * progressionIndex * scalingTernary;
      double selectedObjectsNewDepth = ScaledObjectsInitialDepth + ProgressionInitialZFactor * progressionIndex * scalingTernary;

      xScaleFactor = selectedObjectsNewWidth / ScaledObjectsInitialWidth;
      yScaleFactor = selectedObjectsNewHeight / ScaledObjectsInitialHeight;
      zScaleFactor = selectedObjectsNewDepth / ScaledObjectsInitialDepth;

      var progressionTriangularNumber = (progressionIndex * progressionIndex + progressionIndex) / 2;
      var progressionYTranslation = ScaledObjectsInitialHeight * progressionIndex * scalingTernary + ProgressionInitialYFactor * progressionTriangularNumber + IncreasingSpacing * progressionIndex * scalingTernary;

      translationTransformation.M13 += progressionYTranslation;
    }

    translationTransformation.M03 += -xScaleFactor * ScaledObjectsInitialXCenter + ScaledObjectsInitialXCenter;
    translationTransformation.M13 += -yScaleFactor * ScaledObjectsInitialYCenter + ScaledObjectsInitialYCenter;
    translationTransformation.M23 += -zScaleFactor * ScaledObjectsInitialZCenter + ScaledObjectsInitialZCenter;

    scalingTransformation.M00 = xScaleFactor;
    scalingTransformation.M11 = yScaleFactor;
    scalingTransformation.M22 = zScaleFactor;

    if (xScaleFactor <= 0) return;
    if (yScaleFactor <= 0) return;
    if (zScaleFactor <= 0) return;

    if (scalingMode == ScalingModes.Definition || scalingMode == ScalingModes.Contraction) {
      foreach (var rhinoObject in SelectedObjects) {
        rhinoObject.Geometry.Transform(scalingTransformation);
        rhinoObject.CommitChanges();
      }

      SelectedObjectsActualWidth = xScaleFactor * SelectedObjectsActualWidth;
      SelectedObjectsActualHeight = yScaleFactor * SelectedObjectsActualHeight;
      SelectedObjectsActualDepth = zScaleFactor * SelectedObjectsActualDepth;

      HasScaledByDefinitionOrContraction = true;
    }

    if (scalingMode == ScalingModes.Progression && !HasScaledByDefinitionOrContraction) {
      foreach (var rhinoObject in SelectedObjects) {
        var rhinoObjectGeometryCopy = rhinoObject.Geometry.Duplicate();

        rhinoObjectGeometryCopy.Transform(scalingTransformation);
        rhinoObjectGeometryCopy.Transform(translationTransformation);

        RhinoDocument.Objects.Add(rhinoObjectGeometryCopy);
      }
    }

    RhinoDocument.Views.Redraw();
  }

  public void DeleteSelectedObjects() {
    foreach (var selectedObject in SelectedObjects) {
      RhinoDocument.Objects.Delete(selectedObject);
    }

    RhinoDocument.Views.Redraw();
  }

  public ExcaliburScaler Reinstantiate() {
    return new ExcaliburScaler(RhinoDocument);
  }
}

public class ExcaliburState {
  private readonly Rhino.RhinoDoc RhinoDocument;

  private int ObjectsLength;

  private Rhino.Geometry.GeometryBase[] InitialObjectsGeometries;
  private Rhino.DocObjects.ObjectAttributes[] InitialObjectsAttributes;

  public bool AreObjectsSet;

  public ExcaliburState(Rhino.RhinoDoc rhinoDocument) {
    RhinoDocument = rhinoDocument;
  }

  public void SetInitialObjects(Rhino.DocObjects.RhinoObject[] rhinoObjects) {
    ObjectsLength = rhinoObjects.Length;

    InitialObjectsGeometries = new Rhino.Geometry.GeometryBase[ObjectsLength];
    InitialObjectsAttributes = new Rhino.DocObjects.ObjectAttributes[ObjectsLength];

    foreach(var rhinoObject in rhinoObjects) {
      var initialObjectGeometry = rhinoObject.Geometry.Duplicate();
      var initialObjectAttributes = rhinoObject.Attributes.Duplicate();

      InitialObjectsGeometries[^1] = initialObjectGeometry;
      InitialObjectsAttributes[^1] = initialObjectAttributes;
    }

    AreObjectsSet = true;
  }

  public void RestoreInitialObjects() {
    for (int i = 0; i < ObjectsLength; i++) {
      RhinoDocument.Objects.Add(InitialObjectsGeometries[i], InitialObjectsAttributes[i]);
    }

    RhinoDocument.Views.Redraw();
  }
}