#include "../inc/PassiveEquipmentUpgradeModelView.h"
void PassiveEquipmentUpgradeModelView::serialize(std::ostream& output) {
	// PassiveAlterations
	output << this->PassiveAlterations.size() << '\n';
	for(int PassiveAlterations_it = 0; PassiveAlterations_it < this->PassiveAlterations.size(); PassiveAlterations_it++) {
		this->PassiveAlterations[PassiveAlterations_it].serialize(output);
	}

	// Cost
	output << ((float)this->Cost) << '\n';
}

PassiveEquipmentUpgradeModelView PassiveEquipmentUpgradeModelView::deserialize(std::istream& input) {
	PassiveEquipmentUpgradeModelView _obj = PassiveEquipmentUpgradeModelView();
	// PassiveAlterations
	std::vector<StateAlterationModelView> _obj_PassiveAlterations = std::vector<StateAlterationModelView>();
	int _obj_PassiveAlterations_count; input >> _obj_PassiveAlterations_count; input.ignore(1000, '\n');
	for(int _obj_PassiveAlterations_i = 0; _obj_PassiveAlterations_i < _obj_PassiveAlterations_count; _obj_PassiveAlterations_i++) {
		StateAlterationModelView _obj_PassiveAlterations_e = StateAlterationModelView::deserialize(input);
		_obj_PassiveAlterations.push_back((StateAlterationModelView)_obj_PassiveAlterations_e);
	}

	_obj.PassiveAlterations = (::std::vector<StateAlterationModelView>)_obj_PassiveAlterations;
	// Cost
	float _obj_Cost; input >> _obj_Cost; input.ignore(1000, '\n');
	_obj.Cost = (float)_obj_Cost;
	return _obj;
}

PassiveEquipmentUpgradeModelView::PassiveEquipmentUpgradeModelView() {
}


