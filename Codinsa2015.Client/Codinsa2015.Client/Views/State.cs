using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Codinsa2015.Views.Client
{

	public class State
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
		/// <summary>
		/// Obtient toutes les données du jeu qui ne vont pas varier lors de son déroulement. A appeler une
		/// fois en PickPhase (pour récup les sorts) et une fois en GamePhase (pour récup les données de la
		/// map)
		/// </summary>
		public GameStaticDataView GetStaticData()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)0).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			GameStaticDataView returnValue = GameStaticDataView.Deserialize(input);
			return (GameStaticDataView)returnValue;
		}
	
		/// <summary>
		/// Lors de la phase de picks, retourne l'action actuellement attendue de la part de ce héros.
		/// </summary>
		public PickAction Picks_NextAction()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)1).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			PickAction returnValue = (PickAction)Int32.Parse(input.ReadLine());
			return (PickAction)returnValue;
		}
	
		/// <summary>
		/// Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells actifs disponibles.
		/// </summary>
		public List<int> Picks_GetActiveSpells()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)2).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<int> returnValue = new List<int>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				int returnValue_e = Int32.Parse(input.ReadLine());
				returnValue.Add((int)returnValue_e);
			}
			return (List<int>)returnValue;
		}
	
		/// <summary>
		/// Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells passifs
		/// disponibles.
		/// </summary>
		public List<EntityUniquePassives> Picks_GetPassiveSpells()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)3).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<EntityUniquePassives> returnValue = new List<EntityUniquePassives>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				EntityUniquePassives returnValue_e = (EntityUniquePassives)Int32.Parse(input.ReadLine());
				returnValue.Add((EntityUniquePassives)returnValue_e);
			}
			return (List<EntityUniquePassives>)returnValue;
		}
	
		/// <summary>
		/// Lors de la phase de picks, permet à l'IA de pick un passif donné (si c'est son tour).
		/// </summary>
		public PickResult Picks_PickPassive(EntityUniquePassives passive)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)4).ToString());
			output.WriteLine(((int)passive).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			PickResult returnValue = (PickResult)Int32.Parse(input.ReadLine());
			return (PickResult)returnValue;
		}
	
		/// <summary>
		/// Lors de la phase de picks, permet à l'IA de pick un spell actif dont l'id est donné (si c'est son
		/// tour).
		/// </summary>
		public PickResult Picks_PickActive(int spell)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)5).ToString());
			output.WriteLine(((int)spell).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			PickResult returnValue = (PickResult)Int32.Parse(input.ReadLine());
			return (PickResult)returnValue;
		}
	
		/// <summary>
		/// Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via
		/// ShopGetWeapons(),ShopGetArmors(), ShopGetBoots() etc...
		/// </summary>
		public ShopTransactionResult ShopPurchaseItem(int equipId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)6).ToString());
			output.WriteLine(((int)equipId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			ShopTransactionResult returnValue = (ShopTransactionResult)Int32.Parse(input.ReadLine());
			return (ShopTransactionResult)returnValue;
		}
	
		/// <summary>
		/// Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor
		/// etc...)
		/// </summary>
		public ShopTransactionResult ShopSell(EquipmentType equipType)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)7).ToString());
			output.WriteLine(((int)equipType).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			ShopTransactionResult returnValue = (ShopTransactionResult)Int32.Parse(input.ReadLine());
			return (ShopTransactionResult)returnValue;
		}
	
		/// <summary>
		/// Effectue une upgrade d'un équipement indiqué en paramètre.
		/// </summary>
		public ShopTransactionResult ShopUpgrade(EquipmentType equipType)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)8).ToString());
			output.WriteLine(((int)equipType).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			ShopTransactionResult returnValue = (ShopTransactionResult)Int32.Parse(input.ReadLine());
			return (ShopTransactionResult)returnValue;
		}
	
		/// <summary>
		/// Obtient la liste des modèles d'armes disponibles au shop.
		/// </summary>
		public List<WeaponModelView> ShopGetWeapons()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)9).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<WeaponModelView> returnValue = new List<WeaponModelView>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				WeaponModelView returnValue_e = WeaponModelView.Deserialize(input);
				returnValue.Add((WeaponModelView)returnValue_e);
			}
			return (List<WeaponModelView>)returnValue;
		}
	
		/// <summary>
		/// Obtient la liste des id des modèles d'armures disponibles au shop.
		/// </summary>
		public List<int> ShopGetArmors()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)10).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<int> returnValue = new List<int>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				int returnValue_e = Int32.Parse(input.ReadLine());
				returnValue.Add((int)returnValue_e);
			}
			return (List<int>)returnValue;
		}
	
		/// <summary>
		/// Obtient la liste des id des modèles de bottes disponibles au shop.
		/// </summary>
		public List<int> ShopGetBoots()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)11).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<int> returnValue = new List<int>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				int returnValue_e = Int32.Parse(input.ReadLine());
				returnValue.Add((int)returnValue_e);
			}
			return (List<int>)returnValue;
		}
	
		/// <summary>
		/// Obtient la liste des id des enchantements disponibles au shop.
		/// </summary>
		public List<int> ShopGetEnchants()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)12).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<int> returnValue = new List<int>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				int returnValue_e = Int32.Parse(input.ReadLine());
				returnValue.Add((int)returnValue_e);
			}
			return (List<int>)returnValue;
		}
	
		/// <summary>
		/// Obtient le nombre de Point d'améliorations du héros.
		/// </summary>
		public float GetMyPA()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)13).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			float returnValue = Single.Parse(input.ReadLine());
			return (float)returnValue;
		}
	
		/// <summary>
		/// Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetMyWeaponId()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)14).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)
		/// </summary>
		public int GetMyWeaponLevel()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)15).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetMyArmorId()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)16).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)
		/// </summary>
		public int GetMyArmorLevel()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)17).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetMyBootsId()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)18).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)
		/// </summary>
		public int GetMyBootsLevel()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)19).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)
		/// </summary>
		public int GetMyWeaponEnchantId()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)20).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Retourne une vue vers le héros contrôlé par ce contrôleur.
		/// </summary>
		public EntityBaseView GetMyHero()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)21).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			EntityBaseView returnValue = EntityBaseView.Deserialize(input);
			return (EntityBaseView)returnValue;
		}
	
		/// <summary>
		/// Retourne la position du héros.
		/// </summary>
		public Vector2 GetMyPosition()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)22).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			Vector2 returnValue = Vector2.Deserialize(input);
			return (Vector2)returnValue;
		}
	
		/// <summary>
		/// Déplace le joueur vers la position donnée en utilisant l'A*.
		/// </summary>
		public bool StartMoveTo(Vector2 position)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)23).ToString());
			position.Serialize(output);
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		/// <summary>
		/// Indique si le joueur est entrain de se déplacer en utilisant son A*.
		/// </summary>
		public bool IsAutoMoving()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)24).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		/// <summary>
		/// Arrête le déplacement automatique (A*) du joueur.
		/// </summary>
		public bool EndMoveTo()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)25).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		/// <summary>
		/// Obtient une valeur indiquant si votre équipe possède la vision à la position donnée.
		/// </summary>
		public bool HasSightAt(Vector2 position)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)26).ToString());
			position.Serialize(output);
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		/// <summary>
		/// Obtient une liste des héros morts.
		/// </summary>
		public List<EntityBaseView> GetDeadHeroes()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)27).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<EntityBaseView> returnValue = new List<EntityBaseView>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				EntityBaseView returnValue_e = EntityBaseView.Deserialize(input);
				returnValue.Add((EntityBaseView)returnValue_e);
			}
			return (List<EntityBaseView>)returnValue;
		}
	
		/// <summary>
		/// Retourne la liste des entités en vue
		/// </summary>
		public List<EntityBaseView> GetEntitiesInSight()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)28).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<EntityBaseView> returnValue = new List<EntityBaseView>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				EntityBaseView returnValue_e = EntityBaseView.Deserialize(input);
				returnValue.Add((EntityBaseView)returnValue_e);
			}
			return (List<EntityBaseView>)returnValue;
		}
	
		/// <summary>
		/// Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès
		/// refusé)
		/// </summary>
		public EntityBaseView GetEntityById(int entityId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)29).ToString());
			output.WriteLine(((int)entityId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			EntityBaseView returnValue = EntityBaseView.Deserialize(input);
			return (EntityBaseView)returnValue;
		}
	
		/// <summary>
		/// Utilise l'arme du héros sur l'entité dont l'id est donné.
		/// </summary>
		public SpellUseResult UseMyWeapon(int entityId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)30).ToString());
			output.WriteLine(((int)entityId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellUseResult returnValue = (SpellUseResult)Int32.Parse(input.ReadLine());
			return (SpellUseResult)returnValue;
		}
	
		/// <summary>
		/// Obtient l'attack range de l'arme du héros au niveau actuel.
		/// </summary>
		public float GetMyAttackRange()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)31).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			float returnValue = Single.Parse(input.ReadLine());
			return (float)returnValue;
		}
	
		/// <summary>
		/// Obtient les points de la trajectoire du héros;
		/// </summary>
		public List<Vector2> GetMyTrajectory()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)32).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<Vector2> returnValue = new List<Vector2>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				Vector2 returnValue_e = Vector2.Deserialize(input);
				returnValue.Add((Vector2)returnValue_e);
			}
			return (List<Vector2>)returnValue;
		}
	
		/// <summary>
		/// Déplace le héros selon la direction donnée.
		/// </summary>
		public bool MoveTowards(Vector2 direction)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)33).ToString());
			direction.Serialize(output);
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		/// <summary>
		/// Obtient les id des spells possédés par le héros.
		/// </summary>
		public List<int> GetMySpells()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)34).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			List<int> returnValue = new List<int>();
			int returnValue_count = Int32.Parse(input.ReadLine());
			for(int returnValue_i = 0; returnValue_i < returnValue_count; returnValue_i++) {
				int returnValue_e = Int32.Parse(input.ReadLine());
				returnValue.Add((int)returnValue_e);
			}
			return (List<int>)returnValue;
		}
	
		/// <summary>
		/// Effectue une upgrade du spell d'id donné (0 ou 1).
		/// </summary>
		public SpellUpgradeResult UpgradeMyActiveSpell(int spellId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)35).ToString());
			output.WriteLine(((int)spellId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellUpgradeResult returnValue = (SpellUpgradeResult)Int32.Parse(input.ReadLine());
			return (SpellUpgradeResult)returnValue;
		}
	
		/// <summary>
		/// Obtient le niveau actuel du spell d'id donné (numéro du spell : 0 ou 1). -1 si le spell n'existe
		/// pas.
		/// </summary>
		public int GetMyActiveSpellLevel(int spellId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)36).ToString());
			output.WriteLine(((int)spellId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Obtient le niveau actuel du spell passif. -1 si erreur.
		/// </summary>
		public int GetMyPassiveSpellLevel()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)37).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		/// <summary>
		/// Effectue une upgrade du spell passif du héros.
		/// </summary>
		public SpellUpgradeResult UpgradeMyPassiveSpell()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)38).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellUpgradeResult returnValue = (SpellUpgradeResult)Int32.Parse(input.ReadLine());
			return (SpellUpgradeResult)returnValue;
		}
	
		/// <summary>
		/// Utilise le sort d'id donné. Retourne true si l'action a été effectuée.
		/// </summary>
		public SpellUseResult UseMySpell(int spellId,SpellCastTargetInfoView target)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)39).ToString());
			output.WriteLine(((int)spellId).ToString());
			target.Serialize(output);
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellUseResult returnValue = (SpellUseResult)Int32.Parse(input.ReadLine());
			return (SpellUseResult)returnValue;
		}
	
		/// <summary>
		/// Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre. (soit 0 soit 1)
		/// </summary>
		public SpellView GetMySpell(int spellId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)40).ToString());
			output.WriteLine(((int)spellId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellView returnValue = SpellView.Deserialize(input);
			return (SpellView)returnValue;
		}
	
		/// <summary>
		/// Obtient la phase actuelle du jeu : Pick (=> phase de picks) ou Game (phase de jeu).
		/// </summary>
		public SceneMode GetMode()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)41).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SceneMode returnValue = (SceneMode)Int32.Parse(input.ReadLine());
			return (SceneMode)returnValue;
		}
	
		/// <summary>
		/// Obtient la description du spell dont l'id est donné en paramètre.
		/// </summary>
		public SpellLevelDescriptionView GetMySpellCurrentLevelDescription(int spellId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)42).ToString());
			output.WriteLine(((int)spellId).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			SpellLevelDescriptionView returnValue = SpellLevelDescriptionView.Deserialize(input);
			return (SpellLevelDescriptionView)returnValue;
		}
	
		public State() {
		}

		public static State Deserialize(System.IO.StreamReader input) {
			State _obj =  new State();
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
		}

	}
}
