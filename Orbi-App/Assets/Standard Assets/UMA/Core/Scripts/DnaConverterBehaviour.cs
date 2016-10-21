using UnityEngine;
using System.Collections;


namespace UMA
{
	/// <summary>
	/// Base class for DNA converters.
	/// </summary>
	public class DnaConverterBehaviour : MonoBehaviour 
	{
		public DnaConverterBehaviour()
		{
			Prepare();
		}
#if WINDOWS_UWP
        //public System.TypeInfo DNAType;
        public System.Type DNAType;
#else
        public System.Type DNAType;
#endif

        public delegate void DNAConvertDelegate(UMAData data, UMASkeleton skeleton);
		/// <summary>
		/// Called on the DNA converter to adjust avatar from DNA values.
		/// </summary>
        public DNAConvertDelegate ApplyDnaAction;

        public virtual void Prepare()
        {
		}
    }
}
