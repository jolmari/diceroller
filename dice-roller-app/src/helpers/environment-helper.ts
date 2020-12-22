// this is so you can access the current runtime environment by using process.env.NODE_ENV,
// to enable the webpack "process.env" plugin
declare let process: {
  env: {
    VUE_APP_BASE_URL: string;
  };
};

export class EnvironmentHelper {
  public static get baseUrl(): string {
    return process.env.VUE_APP_BASE_URL;
  }
}
