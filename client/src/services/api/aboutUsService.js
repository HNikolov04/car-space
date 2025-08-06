import { get, put } from "./httpClient";

export async function getAboutUs() {
  return await get("api/AboutUs"); 
}

export async function updateAboutUs(data) {
  return await put("api/AboutUs", data);
}
