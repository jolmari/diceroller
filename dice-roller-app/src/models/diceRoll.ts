export class DiceRoll {
  constructor(init: Partial<DiceRoll>) {
    Object.assign(this, init);
  }

  public sides = 0;
  public amount = 0;
}
