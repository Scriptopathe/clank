#include "../inc/GameStaticDataView.h"
void GameStaticDataView::serialize(std::ostream& output) {
	// CampsPositions
	output << this->CampsPositions.size() << '\n';
	for(int CampsPositions_it = 0; CampsPositions_it < this->CampsPositions.size(); CampsPositions_it++) {
		this->CampsPositions[CampsPositions_it].serialize(output);
	}

	// RouterPositions
	output << this->RouterPositions.size() << '\n';
	for(int RouterPositions_it = 0; RouterPositions_it < this->RouterPositions.size(); RouterPositions_it++) {
		this->RouterPositions[RouterPositions_it].serialize(output);
	}

	// VirusCheckpoints
	output << this->VirusCheckpoints.size() << '\n';
	for(int VirusCheckpoints_it = 0; VirusCheckpoints_it < this->VirusCheckpoints.size(); VirusCheckpoints_it++) {
		this->VirusCheckpoints[VirusCheckpoints_it].serialize(output);
	}

	// Weapons
	output << this->Weapons.size() << '\n';
	for(int Weapons_it = 0; Weapons_it < this->Weapons.size(); Weapons_it++) {
		this->Weapons[Weapons_it].serialize(output);
	}

	// Armors
	output << this->Armors.size() << '\n';
	for(int Armors_it = 0; Armors_it < this->Armors.size(); Armors_it++) {
		this->Armors[Armors_it].serialize(output);
	}

	// Boots
	output << this->Boots.size() << '\n';
	for(int Boots_it = 0; Boots_it < this->Boots.size(); Boots_it++) {
		this->Boots[Boots_it].serialize(output);
	}

	// Enchants
	output << this->Enchants.size() << '\n';
	for(int Enchants_it = 0; Enchants_it < this->Enchants.size(); Enchants_it++) {
		this->Enchants[Enchants_it].serialize(output);
	}

	// Spells
	output << this->Spells.size() << '\n';
	for(int Spells_it = 0; Spells_it < this->Spells.size(); Spells_it++) {
		this->Spells[Spells_it].serialize(output);
	}

	// Structures
	output << this->Structures.size() << '\n';
	for(int Structures_it = 0; Structures_it < this->Structures.size(); Structures_it++) {
		this->Structures[Structures_it].serialize(output);
	}

	// Map
	this->Map.serialize(output);
}

GameStaticDataView GameStaticDataView::deserialize(std::istream& input) {
	GameStaticDataView _obj = GameStaticDataView();
	// CampsPositions
	std::vector<Vector2> _obj_CampsPositions = std::vector<Vector2>();
	int _obj_CampsPositions_count; input >> _obj_CampsPositions_count; input.ignore(1000, '\n');
	for(int _obj_CampsPositions_i = 0; _obj_CampsPositions_i < _obj_CampsPositions_count; _obj_CampsPositions_i++) {
		Vector2 _obj_CampsPositions_e = Vector2::deserialize(input);
		_obj_CampsPositions.push_back((Vector2)_obj_CampsPositions_e);
	}

	_obj.CampsPositions = (::std::vector<Vector2>)_obj_CampsPositions;
	// RouterPositions
	std::vector<Vector2> _obj_RouterPositions = std::vector<Vector2>();
	int _obj_RouterPositions_count; input >> _obj_RouterPositions_count; input.ignore(1000, '\n');
	for(int _obj_RouterPositions_i = 0; _obj_RouterPositions_i < _obj_RouterPositions_count; _obj_RouterPositions_i++) {
		Vector2 _obj_RouterPositions_e = Vector2::deserialize(input);
		_obj_RouterPositions.push_back((Vector2)_obj_RouterPositions_e);
	}

	_obj.RouterPositions = (::std::vector<Vector2>)_obj_RouterPositions;
	// VirusCheckpoints
	std::vector<EntityBaseView> _obj_VirusCheckpoints = std::vector<EntityBaseView>();
	int _obj_VirusCheckpoints_count; input >> _obj_VirusCheckpoints_count; input.ignore(1000, '\n');
	for(int _obj_VirusCheckpoints_i = 0; _obj_VirusCheckpoints_i < _obj_VirusCheckpoints_count; _obj_VirusCheckpoints_i++) {
		EntityBaseView _obj_VirusCheckpoints_e = EntityBaseView::deserialize(input);
		_obj_VirusCheckpoints.push_back((EntityBaseView)_obj_VirusCheckpoints_e);
	}

	_obj.VirusCheckpoints = (::std::vector<EntityBaseView>)_obj_VirusCheckpoints;
	// Weapons
	std::vector<WeaponModelView> _obj_Weapons = std::vector<WeaponModelView>();
	int _obj_Weapons_count; input >> _obj_Weapons_count; input.ignore(1000, '\n');
	for(int _obj_Weapons_i = 0; _obj_Weapons_i < _obj_Weapons_count; _obj_Weapons_i++) {
		WeaponModelView _obj_Weapons_e = WeaponModelView::deserialize(input);
		_obj_Weapons.push_back((WeaponModelView)_obj_Weapons_e);
	}

	_obj.Weapons = (::std::vector<WeaponModelView>)_obj_Weapons;
	// Armors
	std::vector<PassiveEquipmentModelView> _obj_Armors = std::vector<PassiveEquipmentModelView>();
	int _obj_Armors_count; input >> _obj_Armors_count; input.ignore(1000, '\n');
	for(int _obj_Armors_i = 0; _obj_Armors_i < _obj_Armors_count; _obj_Armors_i++) {
		PassiveEquipmentModelView _obj_Armors_e = PassiveEquipmentModelView::deserialize(input);
		_obj_Armors.push_back((PassiveEquipmentModelView)_obj_Armors_e);
	}

	_obj.Armors = (::std::vector<PassiveEquipmentModelView>)_obj_Armors;
	// Boots
	std::vector<PassiveEquipmentModelView> _obj_Boots = std::vector<PassiveEquipmentModelView>();
	int _obj_Boots_count; input >> _obj_Boots_count; input.ignore(1000, '\n');
	for(int _obj_Boots_i = 0; _obj_Boots_i < _obj_Boots_count; _obj_Boots_i++) {
		PassiveEquipmentModelView _obj_Boots_e = PassiveEquipmentModelView::deserialize(input);
		_obj_Boots.push_back((PassiveEquipmentModelView)_obj_Boots_e);
	}

	_obj.Boots = (::std::vector<PassiveEquipmentModelView>)_obj_Boots;
	// Enchants
	std::vector<WeaponEnchantModelView> _obj_Enchants = std::vector<WeaponEnchantModelView>();
	int _obj_Enchants_count; input >> _obj_Enchants_count; input.ignore(1000, '\n');
	for(int _obj_Enchants_i = 0; _obj_Enchants_i < _obj_Enchants_count; _obj_Enchants_i++) {
		WeaponEnchantModelView _obj_Enchants_e = WeaponEnchantModelView::deserialize(input);
		_obj_Enchants.push_back((WeaponEnchantModelView)_obj_Enchants_e);
	}

	_obj.Enchants = (::std::vector<WeaponEnchantModelView>)_obj_Enchants;
	// Spells
	std::vector<SpellModelView> _obj_Spells = std::vector<SpellModelView>();
	int _obj_Spells_count; input >> _obj_Spells_count; input.ignore(1000, '\n');
	for(int _obj_Spells_i = 0; _obj_Spells_i < _obj_Spells_count; _obj_Spells_i++) {
		SpellModelView _obj_Spells_e = SpellModelView::deserialize(input);
		_obj_Spells.push_back((SpellModelView)_obj_Spells_e);
	}

	_obj.Spells = (::std::vector<SpellModelView>)_obj_Spells;
	// Structures
	std::vector<EntityBaseView> _obj_Structures = std::vector<EntityBaseView>();
	int _obj_Structures_count; input >> _obj_Structures_count; input.ignore(1000, '\n');
	for(int _obj_Structures_i = 0; _obj_Structures_i < _obj_Structures_count; _obj_Structures_i++) {
		EntityBaseView _obj_Structures_e = EntityBaseView::deserialize(input);
		_obj_Structures.push_back((EntityBaseView)_obj_Structures_e);
	}

	_obj.Structures = (::std::vector<EntityBaseView>)_obj_Structures;
	// Map
	MapView _obj_Map = MapView::deserialize(input);
	_obj.Map = (::MapView)_obj_Map;
	return _obj;
}

GameStaticDataView::GameStaticDataView() {
}


