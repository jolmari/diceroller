import { RollResult } from './rollResult';

export class PoolResult {
  constructor(init: Partial<PoolResult>) {
    Object.assign(this, init);
  }

  public playerName = '';
  public timestampFormatted: Date = new Date();
  public diceRolls: RollResult[] = [];
}
