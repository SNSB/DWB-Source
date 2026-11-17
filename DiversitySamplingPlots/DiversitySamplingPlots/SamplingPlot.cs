using System;
using System.Collections.Generic;
using System.Text;

namespace DiversitySamplingPlots
{
    public class SamplingPlot : DiversityWorkbench.SamplingPlot
    {
        #region Parameter

        #region Data

        public static string SqlSamplingPlot = "SELECT PlotID, PartOfPlotID, PlotIdentifier, PlotDescription, InternalNotes, PlotType " +
            "FROM SamplingPlot ";

        public static string SqlSamplingProject = "SELECT PlotID, ProjectID " +
            "FROM SamplingProject ";

        public static string SqlSamplingProjectList = "SELECT ProjectList.Project, ProjectList.ProjectID " +
            "FROM ProjectList INNER JOIN " +
            "SamplingProject ON ProjectList.ProjectID = SamplingProject.ProjectID ";

        public static string SqlSamplingProjectListNoAccess = "SELECT P.Project, P.ProjectID " +
            "FROM SamplingProject, ProjectProxy P " +
            "WHERE P.ProjectID = SamplingProject.ProjectID " +
            "AND SamplingProject.ProjectID NOT IN (SELECT ProjectID FROM ProjectList) ";

        public static string SqlSamplingPlotImage = "SELECT PlotID, URI, ResourceURI, ImageType, Notes " +
            "FROM SamplingPlotImage ";

        public static string SqlSamplingObject = "SELECT SamplingObjectID, DisplayText, SamplingObjectURI, Description " +
            "FROM SamplingObject ";

        public static string SqlSamplingObjectInPlot = "SELECT SamplingObjectID, PlotID " +
            "FROM SamplingObjectInPlot ";

        public static string SqlSamplingPlotPoint = "SELECT PlotID, PointSequence, PointType, PointGroup " +
            "FROM SamplingPlotPoint ";

        public static string SqlSamplingPlotPointInImage = "SELECT PlotID, PointSequence, URI " +
            "FROM SamplingPlotPointInImage";

        public static string SqlSamplingPlotProperty = "SELECT PlotID, PropertyID, PropertyURI, DisplayText, PropertyHierarchyCache, " +
            "PropertyValue, ResponsibleName, ResponsibleAgentURI, Notes, AverageValueCache " +
            "FROM SamplingPlotProperty";

        public static string SqlSamplingPlotLocalisation = "SELECT PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, DeterminationDate, DistanceToLocation, DirectionToLocation, " +
            "ResponsibleName, ResponsibleAgentURI, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache " +
            "FROM SamplingPlotLocalisation";

        public static string SqlSamplingPlotResource = "SELECT PlotID, ResourceURI, ResponsibleName, ResponsibleAgentURI, Notes " +
            "FROM SamplingPlotResource";

        #endregion

        #endregion

        #region Construction
        public SamplingPlot(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        #endregion

    }
}
