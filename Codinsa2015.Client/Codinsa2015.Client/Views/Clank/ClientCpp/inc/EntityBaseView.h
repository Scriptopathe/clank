#pragma once
#include "Common.h"
#include "StateAlterationView.h"
#include "Vector2.h"


class EntityBaseView
{

public: 
	// Retourne la résistance magique effective de cette entité.
	float GetMagicResist;
	// Retourne la valeur d'AP effective de cette entité.
	float GetAbilityPower;
	// Retourne la valeur de CDR effective de cette entité.
	float GetCooldownReduction;
	// Obtient la vitesse de déplacement de l'entité.
	float GetMoveSpeed;
	// Obtient la vitesse d'attaque effective de l'entité.
	float GetAttackSpeed;
	// Obtient les points d'attaque effectifs de cette entité.
	float GetAttackDamage;
	// Fonction utilisée pour obtenir les points d'armure effectifs sur cette unité.
	float GetArmor;
	// Obtient les HP actuels de cette entité.
	float GetHP;
	// Obtient les HP max actuels de cette entité.
	float GetMaxHP;
	// Si cette entité est un héros, obtient le rôle de ce héros.
	EntityHeroRole Role;
	// Obtient la liste des altérations d'état affectées à cette entité.
	std::vector<StateAlterationView> StateAlterations;
	// Représente les points d'armure de base de cette entité.
	float BaseArmor;
	// Représente la direction de cette entité.
	Vector2 Direction;
	// Position de l'entité sur la map.
	Vector2 Position;
	// Points de bouclier de cette entité.
	float ShieldPoints;
	// Obtient les points de vie actuels de l'entité
	float HP;
	// Obtient le nombre de points de vie maximum de base de cette entité.
	float BaseMaxHP;
	// Obtient la vitesse de déplacement de base de l'entité.
	float BaseMoveSpeed;
	// Retourne une valeur indiquant si l'entité est morte.
	bool IsDead;
	// Retourne le type de cette entité.
	EntityType Type;
	// Obtient l'id de cette entité.
	int ID;
	// Obtient ou définit les points d'attaque de base de cette unité.
	float BaseAttackDamage;
	// Cooldown reduction de base de cette unité.
	float BaseCooldownReduction;
	// Attack speed de base de cette entité.
	float BaseAttackSpeed;
	// Points d'AP de base de cette entité.
	float BaseAbilityPower;
	// Point de résistance magique de base de cette entité.
	float BaseMagicResist;
	// Obtient une valeur indiquant si cette entité est Rootée. (ne peut plus bouger).
	bool IsRooted;
	// Obtient une valeur indiquant si cette unité est Silenced (ne peut pas utiliser de sorts).
	bool IsSilenced;
	// Obtient une valeur indiquant si cette unité est Stuned (ne peut pas bouger ni utiliser de
	// sorts).
	bool IsStuned;
	// Obtient une valeur indiquant si cette unité est invisible.
	bool IsStealthed;
	// Obtient une valeur indiquant si cette entité possède la vision pure.
	bool HasTrueVision;
	// Obtient une valeur indiquant si cette unité peut voir les wards.
	bool HasWardVision;
	// Retourne la range à laquelle cette entité donne la vision.
	float VisionRange;
	void serialize(std::ostream& output);

	static EntityBaseView deserialize(std::istream& input);
private: 

};