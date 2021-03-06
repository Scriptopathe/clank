using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Codinsa2015.Views.Client
{

	public class Vector2
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}	
		public float X;	
		public float Y;	
		public Vector2() {
		}

		public static Vector2 Deserialize(System.IO.StreamReader input) {
			Vector2 _obj =  new Vector2();
			// X
			float _obj_X = Single.Parse(input.ReadLine());
			_obj.X = (float)_obj_X;
			// Y
			float _obj_Y = Single.Parse(input.ReadLine());
			_obj.Y = (float)_obj_Y;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// X
			output.WriteLine(((float)this.X).ToString());
			// Y
			output.WriteLine(((float)this.Y).ToString());
		}

	}
}
