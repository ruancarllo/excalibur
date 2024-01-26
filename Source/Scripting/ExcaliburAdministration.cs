[assembly: System.Runtime.InteropServices.Guid("8f544372-c3fd-46ee-801b-f6b426b6c0a1")]

namespace ExcaliburAdministration {
  public class ExternalPlugIn: Rhino.PlugIns.PlugIn {
    public static ExternalPlugIn Instance {
      get;
      private set;
    }

    public ExternalPlugIn() {
      Instance = this;
    }  
  }

  public class ExternalCommand: Rhino.Commands.Command {
    public static ExternalCommand Instance {
      get;
      private set;
    }

    public override System.String EnglishName {
      get {
        return "Excalibur";
      }
    }

    public ExternalCommand() {
      Instance = this;
    }

    protected override Rhino.Commands.Result RunCommand(Rhino.RhinoDoc activeDocument, Rhino.Commands.RunMode runMode) {
      new ExcaliburAdministration.InternalInstance(activeDocument);
      return Rhino.Commands.Result.Success;
    }
  }

  public class InternalInstance {
    private readonly ExcaliburComponents.MainWindow MainWindow;

    private ExcaliburProcessing.SelectionState SelectionState;
    private readonly ExcaliburProcessing.OperationsState OperationsState;

    public InternalInstance(Rhino.RhinoDoc activeDocument) {
      MainWindow = new ExcaliburComponents.MainWindow();

      SelectionState = new ExcaliburProcessing.SelectionState(activeDocument);
      OperationsState = new ExcaliburProcessing.OperationsState(activeDocument);

      MainWindow.Shown += StartListeningEvents;
      MainWindow.FormClosed += StopListeningEvents;

      System.Windows.Forms.Application.EnableVisualStyles();
      MainWindow.Show();

      SetCurrentDimensionTextBoxes();
    }

    private void StartListeningEvents(System.Object sender, System.EventArgs eventArgs) {
      MainWindow.ColorButtonForX.Click += HandleCopyEventForX;
      MainWindow.ColorButtonForY.Click += HandleCopyEventForY;
      MainWindow.ColorButtonForZ.Click += HandleCopyEventForZ;

      MainWindow.MediumButtonForUndo.Click += HandleUndoEvent;
      MainWindow.MediumButtonForApply.Click += HandleApplyEvent;

      MainWindow.SmallButtonForSuperdilate.Click += HandleSuperdilateEvent;
      MainWindow.SmallButtonForSubdilate.Click += HandleSubdilateEvent;

      MainWindow.LargeButtonForSpread.Click += HandleSpreadEvent;

      Rhino.RhinoDoc.SelectObjects += HandleSelectionEvent;
    }

    private void StopListeningEvents(System.Object sender, System.EventArgs eventArgs) {
      MainWindow.ColorButtonForX.Click -= HandleCopyEventForX;
      MainWindow.ColorButtonForY.Click -= HandleCopyEventForY;
      MainWindow.ColorButtonForZ.Click -= HandleCopyEventForZ;

      MainWindow.MediumButtonForUndo.Click -= HandleUndoEvent;
      MainWindow.MediumButtonForApply.Click -= HandleApplyEvent;

      MainWindow.SmallButtonForSuperdilate.Click -= HandleSuperdilateEvent;
      MainWindow.SmallButtonForSubdilate.Click -= HandleSubdilateEvent;

      MainWindow.LargeButtonForSpread.Click -= HandleSpreadEvent;

      Rhino.RhinoDoc.SelectObjects -= HandleSelectionEvent;
    }

    private void HandleSelectionEvent(System.Object sender, System.EventArgs eventArgs) {
      SelectionState = new ExcaliburProcessing.SelectionState(SelectionState.ActiveDocument);
      
      SetCurrentDimensionTextBoxes();
    }

    private void HandleCopyEventForX(System.Object sender, System.EventArgs eventArgs) {
      MainWindow.MediumWritableTextBoxForX.Text = MainWindow.ReadOnlyTextBoxForWidth.Text;
    }

    private void HandleCopyEventForY(System.Object sender, System.EventArgs eventArgs) {
      MainWindow.MediumWritableTextBoxForY.Text = MainWindow.ReadOnlyTextBoxForHeight.Text;
    }

    private void HandleCopyEventForZ(System.Object sender, System.EventArgs eventArgs) {
      MainWindow.MediumWritableTextBoxForZ.Text = MainWindow.ReadOnlyTextBoxForDepth.Text;
    }

    private void HandleUndoEvent(System.Object sender, System.EventArgs eventArgs) {
      OperationsState.RevertLastChange();
    }

    private void HandleApplyEvent(System.Object sender, System.EventArgs eventArgs) {
      if (MainWindow.CheckBoxForPercentual.Checked == false) {
        if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForX.Text, out System.Double newWidth)) {
          newWidth = SelectionState.LastWidth;
        }

        if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForY.Text, out System.Double newHeight)) {
          newHeight = SelectionState.LastHeight;
        }

        if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForZ.Text, out System.Double newDepth)) {
          newDepth = SelectionState.LastDepth;
        }

        System.Boolean wasDilationSuccessful = ExcaliburProcessing.DilatorMachine.DilateObjectsByDefinition(
          SelectionState.ActiveDocument,
          SelectionState.SelectedObjects,
          newWidth,
          newHeight,
          newDepth,
          out System.Collections.Generic.List<System.Guid> primaryObjectGuids,
          out Rhino.Geometry.Transform inverseTransformation
        );

        if (wasDilationSuccessful) {
          OperationsState.RegisterModifyingChange(primaryObjectGuids, inverseTransformation);
        }
      }

      if (MainWindow.CheckBoxForPercentual.Checked == true) {
        if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForX.Text, out System.Double contractionFactorForX)) {
          contractionFactorForX = 0;
        }

        if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForY.Text, out System.Double contractionFactorForY)) {
          contractionFactorForY = 0;
        }

        if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForZ.Text, out System.Double contractionFactorForZ)) {
          contractionFactorForZ = 0;
        }

        System.Boolean wasDilationSuccessful = ExcaliburProcessing.DilatorMachine.DilateObjectsByContraction(
          SelectionState.ActiveDocument,
          SelectionState.SelectedObjects,
          contractionFactorForX,
          contractionFactorForY,
          contractionFactorForZ,
          out System.Collections.Generic.List<System.Guid> primaryObjectGuids,
          out Rhino.Geometry.Transform inverseTransformation
        );

        if (wasDilationSuccessful) {
          OperationsState.RegisterModifyingChange(primaryObjectGuids, inverseTransformation);
        }
      }
    }

    private void HandleSubdilateEvent(System.Object sender, System.EventArgs eventArgs) {
      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForX.Text, out System.Double progressionFactorForX)) {
        progressionFactorForX = 0;
      }

      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForY.Text, out System.Double progressionFactorForY)) {
        progressionFactorForY = 0;
      }

      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForZ.Text, out System.Double progressionFactorForZ)) {
        progressionFactorForZ = 0;
      }

      System.Boolean wasDilationSuccessful = ExcaliburProcessing.DilatorMachine.DilateObjectsByProgression(
        SelectionState.ActiveDocument,
        SelectionState.LastSubdilatedObjects,
        progressionFactorForX,
        progressionFactorForY,
        progressionFactorForZ,
        -1,
        out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> newPrimaryObjects,
        out System.Collections.Generic.List<System.Guid> newPrimaryObjectGuids
      );

      if (wasDilationSuccessful) {
        SelectionState.LastSubdilatedObjects = newPrimaryObjects;
        OperationsState.RegisterCreativeChange(newPrimaryObjectGuids);
      }
    }

    private void HandleSuperdilateEvent(System.Object sender, System.EventArgs eventArgs) {
      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForX.Text, out System.Double progressionFactorForX)) {
        progressionFactorForX = 0;
      }

      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForY.Text, out System.Double progressionFactorForY)) {
        progressionFactorForY = 0;
      }

      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForZ.Text, out System.Double progressionFactorForZ)) {
        progressionFactorForZ = 0;
      }

      System.Boolean wasDilationSuccessful = ExcaliburProcessing.DilatorMachine.DilateObjectsByProgression(
        SelectionState.ActiveDocument,
        SelectionState.LastSuperdilatedObjects,
        progressionFactorForX,
        progressionFactorForY,
        progressionFactorForZ,
        +1,
        out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> newPrimaryObjects,
        out System.Collections.Generic.List<System.Guid> newPrimaryObjectGuids
      );

      if (wasDilationSuccessful) {
        SelectionState.LastSuperdilatedObjects = newPrimaryObjects;
        OperationsState.RegisterCreativeChange(newPrimaryObjectGuids);
      }
    }

    private void HandleSpreadEvent(System.Object sender, System.EventArgs eventArgs) {
      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.SmallWritableTextBoxForReference.Text, out System.Double spreadingReferenceValue)) {
        return;
      }

      System.Collections.Generic.List<System.Double> spreadingValues = new System.Collections.Generic.List<System.Double>();

      System.String[] spreadingValueLines = MainWindow.HighWritableTextBoxForValues.Text.Trim().Split(
        new System.String[] { System.Environment.NewLine },
        System.StringSplitOptions.None
      );

      foreach (System.String spreadingValueLine in spreadingValueLines) {
        if (ExcaliburUtilities.GeneralTools.ParseNumericText(spreadingValueLine, out System.Double spreadingValue)) {
          spreadingValues.Add(spreadingValue);
        }
      }

      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForX.Text, out System.Double spreadingFactorForX)) {
        spreadingFactorForX = 0;
      }

      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForY.Text, out System.Double spreadingFactorForY)) {
        spreadingFactorForY = 0;
      }

      if (!ExcaliburUtilities.GeneralTools.ParseNumericText(MainWindow.MediumWritableTextBoxForZ.Text, out System.Double spreadingFactorForZ)) {
        spreadingFactorForZ = 0;
      }

      System.Boolean wasDilationSuccessful = ExcaliburProcessing.DilatorMachine.DilateObjectsBySpreading(
        SelectionState.ActiveDocument,
        SelectionState.SelectedObjects,
        spreadingFactorForX,
        spreadingFactorForY,
        spreadingFactorForZ,
        spreadingReferenceValue,
        spreadingValues,
        out System.Collections.Generic.List<System.Guid> newPrimaryObjectGuids
      );

      if (wasDilationSuccessful) {
        OperationsState.RegisterCreativeChange(newPrimaryObjectGuids);
      }
    }

    private void SetCurrentDimensionTextBoxes() {
      MainWindow.ReadOnlyTextBoxForWidth.Text = SelectionState.LastWidth.ToString();
      MainWindow.ReadOnlyTextBoxForHeight.Text = SelectionState.LastHeight.ToString();
      MainWindow.ReadOnlyTextBoxForDepth.Text = SelectionState.LastDepth.ToString();
    }
  }
}