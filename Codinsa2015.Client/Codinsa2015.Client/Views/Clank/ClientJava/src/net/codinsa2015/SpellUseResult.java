package net.codinsa2015;
public enum SpellUseResult
{
	Success(0),
	InvalidTarget(1),
	InvalidTargettingType(2),
	OnCooldown(3),
	Silenced(4),
	Blind(5),
	OutOfRange(6),
	InvalidOperation(7);
	int _value;
	SpellUseResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static SpellUseResult fromValue(int value) { 
		switch(value) { 
			case 0: return Success;
			case 1: return InvalidTarget;
			case 2: return InvalidTargettingType;
			case 3: return OnCooldown;
			case 4: return Silenced;
			case 5: return Blind;
			case 6: return OutOfRange;
			case 7: return InvalidOperation;
		}
		return Success;
	}
}
