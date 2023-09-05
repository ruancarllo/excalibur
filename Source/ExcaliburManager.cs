
[assembly: System.Runtime.InteropServices.Guid("8f544372-c3fd-46ee-801b-f6b426b6c0a1")]

namespace ExcaliburManager {
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

    public override string EnglishName => "Excalibur";

    protected override Rhino.Commands.Result RunCommand(Rhino.RhinoDoc rhinoDocument, Rhino.Commands.RunMode mode) {
      new ExcaliburHandler();
      return Rhino.Commands.Result.Success;
    }
  }

  public class ExcaliburHandler {
    private readonly ExcaliburUI.ExcaliburWindow Window;
    private ExcaliburMetrics.ExcaliburDilator Dilator;
    private ExcaliburMetrics.ExcaliburState State;

    public ExcaliburHandler() {
      Window = new ExcaliburUI.ExcaliburWindow();
      
      Dilator = new ExcaliburMetrics.ExcaliburDilator();
      State = new ExcaliburMetrics.ExcaliburState();

      Window.Shown += StartListeningEvents;
      Window.FormClosed += StopListeningEvents;

      System.Windows.Forms.Application.EnableVisualStyles();
      Window.Show();

      SetActualDimensionsTextBoxes();
    }

    public void StartListeningEvents(object sender, System.EventArgs e) {
      Window.CopyButton_X.Click += HandleCopyXButtonEvent;
      Window.CopyButton_Y.Click += HandleCopyYButtonEvent;
      Window.CopyButton_Z.Click += HandleCopyZButtonEvent;

      Window.UndoButton.Click += HandleUndoButtonEvent;
      Window.ApplyButton.Click += HandleApplyButtonEvent;

      Window.SuperdilateButton.Click += HandleSuperdilateButtonEvent;
      Window.SubdilateButton.Click += HandleSubdilateButtonEvent;

      Rhino.RhinoDoc.SelectObjects += HandleSelectionEvent;
    }

    
    public void StopListeningEvents(object sender, System.EventArgs e) {
      Window.CopyButton_X.Click -= HandleCopyXButtonEvent;
      Window.CopyButton_Y.Click -= HandleCopyYButtonEvent;
      Window.CopyButton_Z.Click -= HandleCopyZButtonEvent;

      Window.UndoButton.Click -= HandleUndoButtonEvent;
      Window.ApplyButton.Click -= HandleApplyButtonEvent;

      Window.SuperdilateButton.Click -= HandleSuperdilateButtonEvent;
      Window.SubdilateButton.Click -= HandleSubdilateButtonEvent;

      Rhino.RhinoDoc.SelectObjects -= HandleSelectionEvent;
    }
    
    private void HandleSelectionEvent(object sender, Rhino.DocObjects.RhinoObjectSelectionEventArgs e) {
      if (Dilator.HasJustCommitedChanges) {
        Dilator.HasJustCommitedChanges = false;
      } else {
        State = new ExcaliburMetrics.ExcaliburState();
      }

      Dilator = Dilator.Reinstantiate();

      SetActualDimensionsTextBoxes();
    }

    private void HandleCopyXButtonEvent(object sender, System.EventArgs e) {
      Window.DilationFactorTextBox_X.Text = Window.ActualDimensionTextBox_Width.Text;
    }

    private void HandleCopyYButtonEvent(object sender, System.EventArgs e) {
      Window.DilationFactorTextBox_Y.Text = Window.ActualDimensionTextBox_Height.Text;
    }

    private void HandleCopyZButtonEvent(object sender, System.EventArgs e) {
      Window.DilationFactorTextBox_Z.Text = Window.ActualDimensionTextBox_Depth.Text;
    }

    private void HandleUndoButtonEvent(object sender, System.EventArgs e) {
      if (!State.AreDimensionsHistoryEmpty) {
        Dilator.DilateAnalyzedObjectsByMode(
          ExcaliburMetrics.ExcaliburDilator.ScalingModes.Definition,
          ExcaliburMetrics.ExcaliburDilator.ScalingDirections.None,
          State.LastWidth,
          State.LastHeight,
          State.LastDepth
        );

        State.RemoveLastDimensions();
      }
    }

    private void HandleApplyButtonEvent(object sender, System.EventArgs e) {
      State.AddLastDimensions(Dilator.SelectedObjectsActualWidth, Dilator.SelectedObjectsActualHeight, Dilator.SelectedObjectsActualDepth);

      if (Window.ModeCheckBox_Percentual.Checked == false) {
        if (!double.TryParse(Window.DilationFactorTextBox_X.Text, out double widthDefinition)) widthDefinition = Dilator.SelectedObjectsActualWidth;
        if (!double.TryParse(Window.DilationFactorTextBox_Y.Text, out double heightDefinition)) heightDefinition = Dilator.SelectedObjectsActualHeight;
        if (!double.TryParse(Window.DilationFactorTextBox_Z.Text, out double depthDefinition)) depthDefinition = Dilator.SelectedObjectsActualDepth;

        Dilator.DilateAnalyzedObjectsByMode(
          ExcaliburMetrics.ExcaliburDilator.ScalingModes.Definition,
          ExcaliburMetrics.ExcaliburDilator.ScalingDirections.None,
          widthDefinition,
          heightDefinition,
          depthDefinition
        );
      }

      if (Window.ModeCheckBox_Percentual.Checked == true) {
        if (!double.TryParse(Window.DilationFactorTextBox_X.Text, out double widthContraction)) widthContraction = 0;
        if (!double.TryParse(Window.DilationFactorTextBox_Y.Text, out double heightContraction)) heightContraction = 0;
        if (!double.TryParse(Window.DilationFactorTextBox_Z.Text, out double depthContraction)) depthContraction = 0;

        Dilator.DilateAnalyzedObjectsByMode(
          ExcaliburMetrics.ExcaliburDilator.ScalingModes.Contraction,
          ExcaliburMetrics.ExcaliburDilator.ScalingDirections.None,
          widthContraction,
          heightContraction,
          depthContraction
        );
      }
    }

    private void HandleSuperdilateButtonEvent(object sender, System.EventArgs e) {
      if (!double.TryParse(Window.DilationFactorTextBox_X.Text, out double widthProgression)) widthProgression = 0;
      if (!double.TryParse(Window.DilationFactorTextBox_Y.Text, out double heightProgression)) heightProgression = 0;
      if (!double.TryParse(Window.DilationFactorTextBox_Z.Text, out double depthProgression)) depthProgression = 0;

      Dilator.DilateAnalyzedObjectsByMode(
        ExcaliburMetrics.ExcaliburDilator.ScalingModes.Progression,
        ExcaliburMetrics.ExcaliburDilator.ScalingDirections.Increasing,
        widthProgression,
        heightProgression,
        depthProgression
      );
    }

    private void HandleSubdilateButtonEvent(object sender, System.EventArgs e) {
      if (!double.TryParse(Window.DilationFactorTextBox_X.Text, out double widthProgression)) widthProgression = 0;
      if (!double.TryParse(Window.DilationFactorTextBox_Y.Text, out double heightProgression)) heightProgression = 0;
      if (!double.TryParse(Window.DilationFactorTextBox_Z.Text, out double depthProgression)) depthProgression = 0;

      Dilator.DilateAnalyzedObjectsByMode(
        ExcaliburMetrics.ExcaliburDilator.ScalingModes.Progression,
        ExcaliburMetrics.ExcaliburDilator.ScalingDirections.Decreasing,
        widthProgression,
        heightProgression,
        depthProgression
      );
    }

    private void SetActualDimensionsTextBoxes() {
      Window.ActualDimensionTextBox_Width.Text = Dilator.SelectedObjectsActualWidth.ToString();
      Window.ActualDimensionTextBox_Height.Text = Dilator.SelectedObjectsActualHeight.ToString();
      Window.ActualDimensionTextBox_Depth.Text = Dilator.SelectedObjectsActualDepth.ToString();
    }
  }
}