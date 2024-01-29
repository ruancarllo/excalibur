namespace ExcaliburProcessing {
  public class DilatorMachine {
    public static System.Boolean DilateObjectsByDefinition(
      Rhino.RhinoDoc activeDocument,
      System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> primaryObjects,
      System.Double newWidth,
      System.Double newHeight,
      System.Double newDepth,
      out System.Collections.Generic.List<System.Guid> primaryObjectGuids,
      out Rhino.Geometry.Transform inverseTransformation
    ) {
      ComputeObjectsInformations(
        activeDocument,
        primaryObjects,
        out primaryObjectGuids,
        out System.Double primaryObjectsWidth,
        out System.Double primaryObjectsHeight,
        out System.Double primaryObjectsDepth,
        out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> relatedObjects,
        out Rhino.Geometry.Point3d relatedObjectsCenter,
        out System.Double relatedObjectsWidth,
        out System.Double relatedObjectsHeight,
        out System.Double relatedObjectsDepth
      );

      System.Double dilationFactorForX = newWidth / primaryObjectsWidth;
      System.Double dilationFactorForY = newHeight / primaryObjectsHeight;
      System.Double dilationFactorForZ = newDepth / primaryObjectsDepth;

      Rhino.Geometry.Transform dilationTransformation = Rhino.Geometry.Transform.Identity;

      dilationTransformation.M00 = dilationFactorForX;
      dilationTransformation.M11 = dilationFactorForY;
      dilationTransformation.M22 = dilationFactorForZ;

      System.Boolean isTransformationInversible = dilationTransformation.TryGetInverse(out inverseTransformation);

      if (dilationFactorForX <= 0) return false;
      if (dilationFactorForY <= 0) return false;
      if (dilationFactorForZ <= 0) return false;

      if (!isTransformationInversible) return false;

      foreach (Rhino.DocObjects.RhinoObject relatedObject in relatedObjects) {
        relatedObject.Geometry.Transform(dilationTransformation);
        relatedObject.CommitChanges();
      }

      activeDocument.Views.Redraw();

      return true;
    }
  
    public static System.Boolean DilateObjectsByContraction(
      Rhino.RhinoDoc activeDocument,
      System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> primaryObjects,
      System.Double contractionFactorForX,
      System.Double contractionFactorForY,
      System.Double contractionFactorForZ,
      out System.Collections.Generic.List<System.Guid> primaryObjectGuids,
      out Rhino.Geometry.Transform inverseTransformation
    ) {
      ComputeObjectsInformations(
        activeDocument,
        primaryObjects,
        out primaryObjectGuids,
        out System.Double primaryObjectsWidth,
        out System.Double primaryObjectsHeight,
        out System.Double primaryObjectsDepth,
        out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> relatedObjects,
        out Rhino.Geometry.Point3d relatedObjectsCenter,
        out System.Double relatedObjectsWidth,
        out System.Double relatedObjectsHeight,
        out System.Double relatedObjectsDepth
      );

      System.Double dilationFactorForX = (contractionFactorForX + 100) / 100;
      System.Double dilationFactorForY = (contractionFactorForY + 100) / 100;
      System.Double dilationFactorForZ = (contractionFactorForZ + 100) / 100;

      Rhino.Geometry.Transform dilationTransformation = Rhino.Geometry.Transform.Identity;

      dilationTransformation.M00 = dilationFactorForX;
      dilationTransformation.M11 = dilationFactorForY;
      dilationTransformation.M22 = dilationFactorForZ;

      System.Boolean isTransformationInversible = dilationTransformation.TryGetInverse(out inverseTransformation);

      if (dilationFactorForX <= 0) return false;
      if (dilationFactorForY <= 0) return false;
      if (dilationFactorForZ <= 0) return false;

      if (!isTransformationInversible) return false;

      foreach (Rhino.DocObjects.RhinoObject relatedOject in relatedObjects) {
        relatedOject.Geometry.Transform(dilationTransformation);
        relatedOject.CommitChanges();
      }

      activeDocument.Views.Redraw();

      return true;
    }

    public static System.Boolean DilateObjectsByProgression(
      Rhino.RhinoDoc activeDocument,
      System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> primaryObjects,
      System.Double progressionFactorForX,
      System.Double progressionFactorForY,
      System.Double progressionFactorForZ,
      System.Int32 progressionDirectionFactor,
      out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> newPrimaryObjects,
      out System.Collections.Generic.List<System.Guid> newPrimaryObjectGuids
    ) {
      newPrimaryObjects = new System.Collections.Generic.List<Rhino.DocObjects.RhinoObject>();
      newPrimaryObjectGuids = new System.Collections.Generic.List<System.Guid>();

      if (progressionDirectionFactor != 1 && progressionDirectionFactor != -1) {
        return false;
      }

      ComputeObjectsInformations(
        activeDocument,
        primaryObjects,
        out System.Collections.Generic.List<System.Guid> primaryObjectGuids,
        out System.Double primaryObjectsWidth,
        out System.Double primaryObjectsHeight,
        out System.Double primaryObjectsDepth,
        out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> relatedObjects,
        out Rhino.Geometry.Point3d relatedObjectsCenter,
        out System.Double relatedObjectsWidth,
        out System.Double relatedObjectsHeight,
        out System.Double relatedObjectsDepth
      );

      System.Double primaryObjectsNewWidth = primaryObjectsWidth + progressionFactorForX * progressionDirectionFactor;
      System.Double primaryObjectsNewHeight = primaryObjectsHeight + progressionFactorForY * progressionDirectionFactor;
      System.Double primaryObjectsNewDepth = primaryObjectsDepth + progressionFactorForZ * progressionDirectionFactor;

      System.Double dilationFactorForX = primaryObjectsNewWidth / primaryObjectsWidth;
      System.Double dilationFactorForY = primaryObjectsNewHeight / primaryObjectsHeight;
      System.Double dilationFactorForZ = primaryObjectsNewDepth / primaryObjectsDepth;

      System.Double relatedObjectsNewHeight = relatedObjectsHeight * dilationFactorForY;

      Rhino.Geometry.Transform dilationTransformation = Rhino.Geometry.Transform.Identity;
      Rhino.Geometry.Transform centralizyingTransformation = Rhino.Geometry.Transform.Identity;
      Rhino.Geometry.Transform translationTransformation = Rhino.Geometry.Transform.Identity;

      centralizyingTransformation.M03 = relatedObjectsCenter.X - (relatedObjectsCenter.X * dilationFactorForX);
      centralizyingTransformation.M13 = relatedObjectsCenter.Y - (relatedObjectsCenter.Y * dilationFactorForY);
      centralizyingTransformation.M23 = relatedObjectsCenter.Z - (relatedObjectsCenter.Z * dilationFactorForZ);

      translationTransformation.M13 = (relatedObjectsNewHeight + relatedObjectsHeight) / 2 * progressionDirectionFactor + ProgressionSpacing * progressionDirectionFactor;

      dilationTransformation.M00 = dilationFactorForX;
      dilationTransformation.M11 = dilationFactorForY;
      dilationTransformation.M22 = dilationFactorForZ;
      
      if (dilationFactorForX <= 0) return false;
      if (dilationFactorForY <= 0) return false;
      if (dilationFactorForZ <= 0) return false;

      System.Collections.Generic.List<System.Guid> analyzedObjectsGuids = new System.Collections.Generic.List<System.Guid>();

      System.Int32 newGroupIndex = activeDocument.Groups.Add();

      foreach (Rhino.DocObjects.RhinoObject relatedOject in relatedObjects) {
        if (!analyzedObjectsGuids.Contains(relatedOject.Id)) {
          Rhino.Geometry.GeometryBase relatedObjectGeometryCopy = relatedOject.Geometry.Duplicate();
          Rhino.DocObjects.ObjectAttributes relatedObjectAttributesCopy = relatedOject.Attributes.Duplicate();

          relatedObjectAttributesCopy.RemoveFromAllGroups();

          relatedObjectGeometryCopy.Transform(dilationTransformation);
          relatedObjectGeometryCopy.Transform(centralizyingTransformation);
          relatedObjectGeometryCopy.Transform(translationTransformation);

          System.Guid newObjectGuid = activeDocument.Objects.Add(relatedObjectGeometryCopy, relatedObjectAttributesCopy);
          Rhino.DocObjects.RhinoObject newObject = activeDocument.Objects.FindId(newObjectGuid);

          activeDocument.Groups.AddToGroup(newGroupIndex, newObjectGuid);

          if (primaryObjectGuids.Contains(relatedOject.Id)) {
            newPrimaryObjects.Add(newObject);
            newPrimaryObjectGuids.Add(newObjectGuid);
          }

          analyzedObjectsGuids.Add(relatedOject.Id);
        }
      }

      activeDocument.Views.Redraw();

      return true;
    }

    public static System.Boolean DilateObjectsBySpreading(
      Rhino.RhinoDoc activeDocument,
      System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> primaryObjects,
      System.Double spreadingFactorForX,
      System.Double spreadingFactorForY,
      System.Double spreadingFactorForZ,
      System.Double spreadingReferenceValue,
      System.Collections.Generic.List<System.Double> spreadingValues,
      out System.Collections.Generic.List<System.Guid> newPrimaryObjectGuids
    ) {
      newPrimaryObjectGuids = new System.Collections.Generic.List<System.Guid>();

      System.Collections.Generic.List<System.Double> lesserSpreadingValues = new System.Collections.Generic.List<System.Double>();
      System.Collections.Generic.List<System.Double> afferoSpreadingValues = new System.Collections.Generic.List<System.Double>();

      foreach (System.Double spreadingValue in spreadingValues) {
        if (spreadingValue < spreadingReferenceValue) {
          lesserSpreadingValues.Add(spreadingValue);
        }

        if (spreadingValue > spreadingReferenceValue) {
          afferoSpreadingValues.Add(spreadingValue);
        }
      }

      lesserSpreadingValues.Sort();
      lesserSpreadingValues.Reverse();
      afferoSpreadingValues.Sort();

      System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> lastPrimaryObjects = primaryObjects;

      for (System.Int32 lesserIndex = 0; lesserIndex < lesserSpreadingValues.Count; lesserIndex++) {
        System.Double currentLesserSpreadingValue = lesserSpreadingValues[lesserIndex];
        System.Double lastLesserSpreadingValue = lesserIndex == 0 ? spreadingReferenceValue : lesserSpreadingValues[lesserIndex - 1];

        System.Double progressionFactorForX = spreadingFactorForX * (lastLesserSpreadingValue - currentLesserSpreadingValue);
        System.Double progressionFactorForY = spreadingFactorForY * (lastLesserSpreadingValue - currentLesserSpreadingValue);
        System.Double progressionFactorForZ = spreadingFactorForZ * (lastLesserSpreadingValue - currentLesserSpreadingValue);

        System.Boolean wasDilationSuccessful = ExcaliburProcessing.DilatorMachine.DilateObjectsByProgression(
          activeDocument,
          lastPrimaryObjects,
          progressionFactorForX,
          progressionFactorForY,
          progressionFactorForZ,
          -1,
          out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> newPrimaryObjects,
          out System.Collections.Generic.List<System.Guid> newLastPrimaryObjectGuids
        );

        if (wasDilationSuccessful) {
          lastPrimaryObjects = newPrimaryObjects;
          newPrimaryObjectGuids.AddRange(newLastPrimaryObjectGuids);
        }

        else {
          return false;
        }
      }

      lastPrimaryObjects = primaryObjects;

      for (System.Int32 afferoIndex = 0; afferoIndex < afferoSpreadingValues.Count; afferoIndex++) {
        System.Double currentAfferoSpreadingValue = afferoSpreadingValues[afferoIndex];
        System.Double lastAfferoSpreadingValue = afferoIndex == 0 ? spreadingReferenceValue : afferoSpreadingValues[afferoIndex - 1];

        System.Double progressionFactorForX = spreadingFactorForX * (currentAfferoSpreadingValue - lastAfferoSpreadingValue);
        System.Double progressionFactorForY = spreadingFactorForY * (currentAfferoSpreadingValue - lastAfferoSpreadingValue);
        System.Double progressionFactorForZ = spreadingFactorForZ * (currentAfferoSpreadingValue - lastAfferoSpreadingValue);

        System.Boolean wasDilationSuccessful = ExcaliburProcessing.DilatorMachine.DilateObjectsByProgression(
          activeDocument,
          lastPrimaryObjects,
          progressionFactorForX,
          progressionFactorForY,
          progressionFactorForZ,
          +1,
          out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> newPrimaryObjects,
          out System.Collections.Generic.List<System.Guid> newLastPrimaryObjectGuids
        );

        if (wasDilationSuccessful) {
          lastPrimaryObjects = newPrimaryObjects;
          newPrimaryObjectGuids.AddRange(newLastPrimaryObjectGuids);
        }

        else {
          return false;
        }
      }

      return true;
    }

    public static void ComputeObjectsInformations(
      Rhino.RhinoDoc activeDocument,
      System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> primaryObjects,
      out System.Collections.Generic.List<System.Guid> primaryObjectGuids,
      out System.Double primaryObjectsWidth,
      out System.Double primaryObjectsHeight,
      out System.Double primaryObjectsDepth,
      out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> relatedObjects,
      out Rhino.Geometry.Point3d relatedObjectsCenter,
      out System.Double relatedObjectsWidth,
      out System.Double relatedObjectsHeight,
      out System.Double relatedObjectsDepth
    ) {
      primaryObjectGuids = new System.Collections.Generic.List<System.Guid>();
      System.Collections.Generic.List<System.Guid> colateralObjectGuids = new System.Collections.Generic.List<System.Guid>();

      foreach (Rhino.DocObjects.RhinoObject primaryObject in primaryObjects) {
        primaryObjectGuids.Add(primaryObject.Id);
      }

      foreach (Rhino.DocObjects.RhinoObject primaryObject in primaryObjects) {
        System.Int32[] primaryObjectGroupIndexes = primaryObject.GetGroupList();

        if (primaryObjectGroupIndexes != null) foreach (System.Int32 primaryObjectGroupIndex in primaryObjectGroupIndexes) {
          Rhino.DocObjects.RhinoObject[] objectsInGroup = activeDocument.Objects.FindByGroup(primaryObjectGroupIndex);

          foreach (Rhino.DocObjects.RhinoObject objectInGroup in objectsInGroup) {
            if (!primaryObjectGuids.Contains(objectInGroup.Id)) {
              colateralObjectGuids.Add(objectInGroup.Id);
            }
          }
        }
      }

      relatedObjects = new System.Collections.Generic.List<Rhino.DocObjects.RhinoObject>();

      foreach (System.Guid primaryObjectGuid in primaryObjectGuids) {
        Rhino.DocObjects.RhinoObject primaryObject = activeDocument.Objects.Find(primaryObjectGuid);
        relatedObjects.Add(primaryObject);
      }

      foreach (System.Guid colateralObjectGuid in colateralObjectGuids) {
        Rhino.DocObjects.RhinoObject colateralObject = activeDocument.Objects.Find(colateralObjectGuid);
        relatedObjects.Add(colateralObject);
      }

      Rhino.Geometry.BoundingBox primaryObjectsCombinedBoundingBox = Rhino.Geometry.BoundingBox.Empty;
      
      foreach (Rhino.DocObjects.RhinoObject primaryObject in primaryObjects) {
        Rhino.Geometry.BoundingBox primaryObjectBoundingBox = primaryObject.Geometry.GetBoundingBox(true);
        primaryObjectsCombinedBoundingBox.Union(primaryObjectBoundingBox);
      }

      primaryObjectsWidth = primaryObjectsCombinedBoundingBox.Max.X - primaryObjectsCombinedBoundingBox.Min.X;
      primaryObjectsHeight = primaryObjectsCombinedBoundingBox.Max.Y - primaryObjectsCombinedBoundingBox.Min.Y;
      primaryObjectsDepth = primaryObjectsCombinedBoundingBox.Max.Z - primaryObjectsCombinedBoundingBox.Min.Z;

      if (primaryObjectsWidth < 0) primaryObjectsWidth = 0;
      if (primaryObjectsHeight < 0) primaryObjectsHeight = 0;
      if (primaryObjectsDepth < 0) primaryObjectsDepth = 0;

      Rhino.Geometry.BoundingBox relatedObjectsCombinedBoundingBox = Rhino.Geometry.BoundingBox.Empty;

      foreach (Rhino.DocObjects.RhinoObject relatedObject in relatedObjects) {
        Rhino.Geometry.BoundingBox relatedObjectBoundingBox = relatedObject.Geometry.GetBoundingBox(true);
        relatedObjectsCombinedBoundingBox.Union(relatedObjectBoundingBox);
      }

      relatedObjectsWidth = relatedObjectsCombinedBoundingBox.Max.X - relatedObjectsCombinedBoundingBox.Min.X;
      relatedObjectsHeight = relatedObjectsCombinedBoundingBox.Max.Y - relatedObjectsCombinedBoundingBox.Min.Y;
      relatedObjectsDepth = relatedObjectsCombinedBoundingBox.Max.Z - relatedObjectsCombinedBoundingBox.Min.Z;

      if (relatedObjectsWidth < 0) relatedObjectsWidth = 0;
      if (relatedObjectsHeight < 0) relatedObjectsHeight = 0;
      if (relatedObjectsDepth < 0) relatedObjectsDepth = 0;

      relatedObjectsCenter = relatedObjectsCombinedBoundingBox.Center;
    }

    private readonly static System.Double ProgressionSpacing = 10;
  }

  public class SelectionState {
    public Rhino.RhinoDoc ActiveDocument;

    public System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> SelectedObjects;
    public System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> LastSubdilatedObjects;
    public System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> LastSuperdilatedObjects;

    public System.Double LastWidth;
    public System.Double LastHeight;
    public System.Double LastDepth;

    public SelectionState(Rhino.RhinoDoc activeDocument) {
      ActiveDocument = activeDocument;

      SelectedObjects = new System.Collections.Generic.List<Rhino.DocObjects.RhinoObject>(
        ActiveDocument.Objects.GetSelectedObjects(false, false)
      );

      LastSubdilatedObjects = SelectedObjects;
      LastSuperdilatedObjects = SelectedObjects;

      ExcaliburProcessing.DilatorMachine.ComputeObjectsInformations(
        ActiveDocument,
        SelectedObjects,
        out System.Collections.Generic.List<System.Guid> selectedObjectGuids,
        out System.Double selectedObjectsWidth,
        out System.Double selectedObjectsHeight,
        out System.Double selectedObjectsDepth,
        out System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> relatedObjects,
        out Rhino.Geometry.Point3d relatedObjectsCenter,
        out System.Double relatedObjectsWidth,
        out System.Double relatedObjectsHeight,
        out System.Double relatedObjectsDepth
      );

      LastWidth = selectedObjectsWidth;
      LastHeight = selectedObjectsHeight;
      LastDepth = selectedObjectsDepth;
    }
  }

  public class OperationsState {
    private readonly Rhino.RhinoDoc ActiveDocument;

    private readonly System.Collections.Generic.List<ExcaliburProcessing.OperationsState.ActionTypes> ActionsHistory;
    
    private readonly System.Collections.Generic.List<System.Collections.Generic.List<System.Guid>> ModifyingObjectGuidsListHistory;
    private readonly System.Collections.Generic.List<Rhino.Geometry.Transform> ModifyingTransformationsHistory;

    private readonly System.Collections.Generic.List<System.Collections.Generic.List<System.Guid>> CreativeObjectGuidsListHistory;

    public OperationsState(Rhino.RhinoDoc activeDocument) {
      ActiveDocument = activeDocument;

      ActionsHistory = new System.Collections.Generic.List<ExcaliburProcessing.OperationsState.ActionTypes>();

      ModifyingObjectGuidsListHistory = new System.Collections.Generic.List<System.Collections.Generic.List<System.Guid>>();
      ModifyingTransformationsHistory = new System.Collections.Generic.List<Rhino.Geometry.Transform>();

      CreativeObjectGuidsListHistory = new System.Collections.Generic.List<System.Collections.Generic.List<System.Guid>>();
    }

    public void RegisterModifyingChange(System.Collections.Generic.List<System.Guid> objectGuids, Rhino.Geometry.Transform inverseTransformation) {
      ActionsHistory.Add(ExcaliburProcessing.OperationsState.ActionTypes.Modifying);
      ModifyingTransformationsHistory.Add(inverseTransformation);
      ModifyingObjectGuidsListHistory.Add(objectGuids);
    }

    public void RegisterCreativeChange(System.Collections.Generic.List<System.Guid> objectGuids) {
      ActionsHistory.Add(ExcaliburProcessing.OperationsState.ActionTypes.Creative);
      CreativeObjectGuidsListHistory.Add(objectGuids);
    }

    public void RevertLastChange() {
      if (ActionsHistory.Count - 1 >= 0) {
        ExcaliburProcessing.OperationsState.ActionTypes lastChangeActionType = ActionsHistory[ActionsHistory.Count - 1];

        if (lastChangeActionType == ExcaliburProcessing.OperationsState.ActionTypes.Modifying) {
          System.Collections.Generic.List<System.Guid> lastModifiyngObjectGuids = ModifyingObjectGuidsListHistory[ModifyingObjectGuidsListHistory.Count - 1];
          Rhino.Geometry.Transform lastModifyingTransformation = ModifyingTransformationsHistory[ModifyingTransformationsHistory.Count - 1];

          foreach (System.Guid lastModifyingObjectGuid in lastModifiyngObjectGuids) {
            Rhino.DocObjects.RhinoObject lastModifiyngObject = ActiveDocument.Objects.FindId(lastModifyingObjectGuid);

            lastModifiyngObject.Geometry.Transform(lastModifyingTransformation);
            lastModifiyngObject.CommitChanges();
          }

          ModifyingObjectGuidsListHistory.RemoveAt(ModifyingObjectGuidsListHistory.Count - 1);
          ModifyingTransformationsHistory.RemoveAt(ModifyingTransformationsHistory.Count - 1);
        }

        if (lastChangeActionType == ExcaliburProcessing.OperationsState.ActionTypes.Creative) {
          System.Collections.Generic.List<System.Guid> lastCreativeObjectGuids = CreativeObjectGuidsListHistory[CreativeObjectGuidsListHistory.Count - 1];

          foreach (System.Guid lastCreativeObjectGuid in lastCreativeObjectGuids) {
            ActiveDocument.Objects.Delete(lastCreativeObjectGuid, true);
          }

          System.Collections.Generic.List<Rhino.DocObjects.RhinoObject> selectedObjects = new System.Collections.Generic.List<Rhino.DocObjects.RhinoObject>(
            ActiveDocument.Objects.GetSelectedObjects(false, false)
          );

          foreach (Rhino.DocObjects.RhinoObject selectedObject in selectedObjects) {
            selectedObject.Select(false);
          }

          CreativeObjectGuidsListHistory.RemoveAt(CreativeObjectGuidsListHistory.Count - 1);
        }

        ActionsHistory.RemoveAt(ActionsHistory.Count - 1);

        ActiveDocument.Views.Redraw();
      }
    }

    public enum ActionTypes {
      Modifying,
      Creative
    }
  }
}