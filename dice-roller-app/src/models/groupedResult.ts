export class RollMap {
  [sides: number]: number[];
}
export class GroupedResult {
  constructor(init: Partial<GroupedResult>) {
    Object.assign(this, init);
  }

  public playerName = '';
  public rolls: RollMap = new RollMap();
}
