import axios, {AxiosInstance} from "axios"

export abstract class BaseApi {
  protected instance: AxiosInstance;

  protected constructor(
    baseURL: string | undefined
  ) {
    this.instance = axios.create({
      baseURL
    })
  }

//   protected get<T>(url: string, payload?: any): Promise<T> {
//     if (payload) {
//         const queryString = new URLSearchParams(payload).toString();
//         url = `${url}?${queryString}`;
//     }

//     return new Promise<T>((resolve, reject) => {
//         this.instance
//             .get(url)
//             .then((response: any) => {
//                 if (response?.data)
//                     resolve(response.data as T)
//                 else
//                     resolve({} as T);
//             })
//             .catch((response: any) => {
//                 reject(response)
//             });
//     })
// }
}