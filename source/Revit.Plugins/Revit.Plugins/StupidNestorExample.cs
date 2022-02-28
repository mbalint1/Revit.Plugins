using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Revit.Plugins
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class StupidNestorExample : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiapp = commandData.Application;
                var doc = uiapp.ActiveUIDocument.Document;

                var sel = uiapp.ActiveUIDocument.Selection;
                var pickedref = sel.PickObject(ObjectType.Element, "Please select a group");
                var elem = doc.GetElement(pickedref);
                var group = elem as Group;

                var point = sel.PickPoint("Please pick a point to place group");

                using (var trans = new Transaction(doc))
                {
                    trans.Start("Lab");
                    doc.Create.PlaceGroup(point, group?.GroupType);
                    trans.Commit();
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                //TODO: Can you write to console or log file?

                return Result.Failed;
            }
        }
    }
}
