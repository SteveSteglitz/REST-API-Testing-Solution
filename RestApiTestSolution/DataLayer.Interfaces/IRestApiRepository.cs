namespace DataLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using DataLayer.Models;

    public interface IRestApiRepository
    {
        IList<String> GetAllProjects(string path);

        ProjectDataModel ReadRestCallFile(string path, string projectName);

        void WriteRestCallFile(string path, ProjectDataModel project);

        void DeleteRestCallFile(string path);
    }
}
