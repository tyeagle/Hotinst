using System;
using System.Collections.Generic;

namespace HOTINST.ICD
{
    public interface IICDManager
    {
        IList<ICDWords> GetAllICD();

        void AddICDWords(ICDWords icdWords);

        void RemoveICDWords(string name);

		[Obsolete]
        void RemoveICDWords(int id);

        ICDWords GetICD(string name);

	    [Obsolete]
		ICDWords GetICD(int id);

	    void Edit(ICDWords icdWords);

	    void Save();
    }
}
