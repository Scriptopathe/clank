package net.codinsa2015;
import java.lang.*;
import java.util.ArrayList;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import net.codinsa2015.Vector2.*;
import java.util.ArrayList;
import net.codinsa2015.EntityBaseView.*;
import net.codinsa2015.WeaponModelView.*;
import net.codinsa2015.PassiveEquipmentModelView.*;
import net.codinsa2015.WeaponEnchantModelView.*;
import net.codinsa2015.SpellModelView.*;
import net.codinsa2015.MapView.*;


@SuppressWarnings("unused")
public class GameStaticDataView
{


	// Obtient la position de tous les camps
	public ArrayList<Vector2> CampsPositions;
	// Obtient la position de tous les routeurs.
	public ArrayList<Vector2> RouterPositions;
	// 
	public ArrayList<EntityBaseView> VirusCheckpoints;
	// Obtient une liste de tous les modèles d'armes du jeu
	public ArrayList<WeaponModelView> Weapons;
	// Obtient une liste de tous les modèles d'armures du jeu
	public ArrayList<PassiveEquipmentModelView> Armors;
	// Obtient une liste de tous les modèles de bottes du jeu
	public ArrayList<PassiveEquipmentModelView> Boots;
	// Obtient une liste de tous les modèles d'enchantements d'arme du jeu
	public ArrayList<WeaponEnchantModelView> Enchants;
	// Obtient une liste de tous les modèles de sorts du jeu.
	public ArrayList<SpellModelView> Spells;
	// Obtient la liste des structures présentes sur la carte. Attention : cette liste n'est pas tenue
	// à jour (statistiques / PV).
	public ArrayList<EntityBaseView> Structures;
	// Obtient une vue sur les données statiques de la carte (telles que sa table de passabilité).
	public MapView Map;
	public GameStaticDataView() {
		CampsPositions = new ArrayList<Vector2>();
		RouterPositions = new ArrayList<Vector2>();
		VirusCheckpoints = new ArrayList<EntityBaseView>();
		Weapons = new ArrayList<WeaponModelView>();
		Armors = new ArrayList<PassiveEquipmentModelView>();
		Boots = new ArrayList<PassiveEquipmentModelView>();
		Enchants = new ArrayList<WeaponEnchantModelView>();
		Spells = new ArrayList<SpellModelView>();
		Structures = new ArrayList<EntityBaseView>();
		Map = new MapView();
	}

	public static GameStaticDataView deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		GameStaticDataView _obj =  new GameStaticDataView();
		// CampsPositions
		ArrayList<Vector2> _obj_CampsPositions = new ArrayList<Vector2>();
		int _obj_CampsPositions_count = Integer.valueOf(input.readLine());
		for(int _obj_CampsPositions_i = 0; _obj_CampsPositions_i < _obj_CampsPositions_count; _obj_CampsPositions_i++) {
			Vector2 _obj_CampsPositions_e = Vector2.deserialize(input);
			_obj_CampsPositions.add((Vector2)_obj_CampsPositions_e);
		}
		_obj.CampsPositions = _obj_CampsPositions;
		// RouterPositions
		ArrayList<Vector2> _obj_RouterPositions = new ArrayList<Vector2>();
		int _obj_RouterPositions_count = Integer.valueOf(input.readLine());
		for(int _obj_RouterPositions_i = 0; _obj_RouterPositions_i < _obj_RouterPositions_count; _obj_RouterPositions_i++) {
			Vector2 _obj_RouterPositions_e = Vector2.deserialize(input);
			_obj_RouterPositions.add((Vector2)_obj_RouterPositions_e);
		}
		_obj.RouterPositions = _obj_RouterPositions;
		// VirusCheckpoints
		ArrayList<EntityBaseView> _obj_VirusCheckpoints = new ArrayList<EntityBaseView>();
		int _obj_VirusCheckpoints_count = Integer.valueOf(input.readLine());
		for(int _obj_VirusCheckpoints_i = 0; _obj_VirusCheckpoints_i < _obj_VirusCheckpoints_count; _obj_VirusCheckpoints_i++) {
			EntityBaseView _obj_VirusCheckpoints_e = EntityBaseView.deserialize(input);
			_obj_VirusCheckpoints.add((EntityBaseView)_obj_VirusCheckpoints_e);
		}
		_obj.VirusCheckpoints = _obj_VirusCheckpoints;
		// Weapons
		ArrayList<WeaponModelView> _obj_Weapons = new ArrayList<WeaponModelView>();
		int _obj_Weapons_count = Integer.valueOf(input.readLine());
		for(int _obj_Weapons_i = 0; _obj_Weapons_i < _obj_Weapons_count; _obj_Weapons_i++) {
			WeaponModelView _obj_Weapons_e = WeaponModelView.deserialize(input);
			_obj_Weapons.add((WeaponModelView)_obj_Weapons_e);
		}
		_obj.Weapons = _obj_Weapons;
		// Armors
		ArrayList<PassiveEquipmentModelView> _obj_Armors = new ArrayList<PassiveEquipmentModelView>();
		int _obj_Armors_count = Integer.valueOf(input.readLine());
		for(int _obj_Armors_i = 0; _obj_Armors_i < _obj_Armors_count; _obj_Armors_i++) {
			PassiveEquipmentModelView _obj_Armors_e = PassiveEquipmentModelView.deserialize(input);
			_obj_Armors.add((PassiveEquipmentModelView)_obj_Armors_e);
		}
		_obj.Armors = _obj_Armors;
		// Boots
		ArrayList<PassiveEquipmentModelView> _obj_Boots = new ArrayList<PassiveEquipmentModelView>();
		int _obj_Boots_count = Integer.valueOf(input.readLine());
		for(int _obj_Boots_i = 0; _obj_Boots_i < _obj_Boots_count; _obj_Boots_i++) {
			PassiveEquipmentModelView _obj_Boots_e = PassiveEquipmentModelView.deserialize(input);
			_obj_Boots.add((PassiveEquipmentModelView)_obj_Boots_e);
		}
		_obj.Boots = _obj_Boots;
		// Enchants
		ArrayList<WeaponEnchantModelView> _obj_Enchants = new ArrayList<WeaponEnchantModelView>();
		int _obj_Enchants_count = Integer.valueOf(input.readLine());
		for(int _obj_Enchants_i = 0; _obj_Enchants_i < _obj_Enchants_count; _obj_Enchants_i++) {
			WeaponEnchantModelView _obj_Enchants_e = WeaponEnchantModelView.deserialize(input);
			_obj_Enchants.add((WeaponEnchantModelView)_obj_Enchants_e);
		}
		_obj.Enchants = _obj_Enchants;
		// Spells
		ArrayList<SpellModelView> _obj_Spells = new ArrayList<SpellModelView>();
		int _obj_Spells_count = Integer.valueOf(input.readLine());
		for(int _obj_Spells_i = 0; _obj_Spells_i < _obj_Spells_count; _obj_Spells_i++) {
			SpellModelView _obj_Spells_e = SpellModelView.deserialize(input);
			_obj_Spells.add((SpellModelView)_obj_Spells_e);
		}
		_obj.Spells = _obj_Spells;
		// Structures
		ArrayList<EntityBaseView> _obj_Structures = new ArrayList<EntityBaseView>();
		int _obj_Structures_count = Integer.valueOf(input.readLine());
		for(int _obj_Structures_i = 0; _obj_Structures_i < _obj_Structures_count; _obj_Structures_i++) {
			EntityBaseView _obj_Structures_e = EntityBaseView.deserialize(input);
			_obj_Structures.add((EntityBaseView)_obj_Structures_e);
		}
		_obj.Structures = _obj_Structures;
		// Map
		MapView _obj_Map = MapView.deserialize(input);
		_obj.Map = _obj_Map;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// CampsPositions
		output.append(String.valueOf(this.CampsPositions.size()) + "\n");
		for(int CampsPositions_it = 0; CampsPositions_it < this.CampsPositions.size();CampsPositions_it++) {
			this.CampsPositions.get(CampsPositions_it).serialize(output);
		}
		// RouterPositions
		output.append(String.valueOf(this.RouterPositions.size()) + "\n");
		for(int RouterPositions_it = 0; RouterPositions_it < this.RouterPositions.size();RouterPositions_it++) {
			this.RouterPositions.get(RouterPositions_it).serialize(output);
		}
		// VirusCheckpoints
		output.append(String.valueOf(this.VirusCheckpoints.size()) + "\n");
		for(int VirusCheckpoints_it = 0; VirusCheckpoints_it < this.VirusCheckpoints.size();VirusCheckpoints_it++) {
			this.VirusCheckpoints.get(VirusCheckpoints_it).serialize(output);
		}
		// Weapons
		output.append(String.valueOf(this.Weapons.size()) + "\n");
		for(int Weapons_it = 0; Weapons_it < this.Weapons.size();Weapons_it++) {
			this.Weapons.get(Weapons_it).serialize(output);
		}
		// Armors
		output.append(String.valueOf(this.Armors.size()) + "\n");
		for(int Armors_it = 0; Armors_it < this.Armors.size();Armors_it++) {
			this.Armors.get(Armors_it).serialize(output);
		}
		// Boots
		output.append(String.valueOf(this.Boots.size()) + "\n");
		for(int Boots_it = 0; Boots_it < this.Boots.size();Boots_it++) {
			this.Boots.get(Boots_it).serialize(output);
		}
		// Enchants
		output.append(String.valueOf(this.Enchants.size()) + "\n");
		for(int Enchants_it = 0; Enchants_it < this.Enchants.size();Enchants_it++) {
			this.Enchants.get(Enchants_it).serialize(output);
		}
		// Spells
		output.append(String.valueOf(this.Spells.size()) + "\n");
		for(int Spells_it = 0; Spells_it < this.Spells.size();Spells_it++) {
			this.Spells.get(Spells_it).serialize(output);
		}
		// Structures
		output.append(String.valueOf(this.Structures.size()) + "\n");
		for(int Structures_it = 0; Structures_it < this.Structures.size();Structures_it++) {
			this.Structures.get(Structures_it).serialize(output);
		}
		// Map
		this.Map.serialize(output);
	}

}
