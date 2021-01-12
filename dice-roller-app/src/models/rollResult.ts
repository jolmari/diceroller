export class RollResult {
  constructor(init: Partial<RollResult>) {
    Object.assign(this, init);
  }

  public sides = 0;
  public amount = 0;
  public results: number[] = [];
}
