#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
namespace Org.BouncyCastle.Asn1.X9
{
	public abstract class X9ECParametersHolder
	{
		private X9ECParameters parameters;

		public X9ECParameters Parameters
		{
			get
			{
				if (parameters == null)
				{
					parameters = CreateParameters();
				}

				return parameters;
			}
		}

		protected abstract X9ECParameters CreateParameters();
	}
}

#endif
