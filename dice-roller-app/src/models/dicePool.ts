import { DiceRoll } from './diceRoll';

export class DicePool {
  constructor(init: Partial<DicePool>) {
    Object.assign(this, init);
  }

  public rolls: DiceRoll[] = [];
}
