import { get, post, put, del } from "./httpClient";

export async function getCarForumArticles(params = {}) {
  const cleanedParams = Object.fromEntries(
    Object.entries(params).filter(([_, value]) => value !== null && value !== undefined)
  );
  const query = new URLSearchParams(cleanedParams).toString();
  return await get(`api/CarForumArticle?${query}`);
}

export async function getCarForumArticleById(id) {
  return await get(`api/CarForumArticle/${id}`);
}

export async function createCarForumArticle(data) {
  return await post("api/CarForumArticle", data);
}

export async function updateCarForumArticle(data) {
  return await put("api/CarForumArticle", data);
}

export async function deleteCarForumArticle(id) {
  return await del(`api/CarForumArticle/${id}`);
}

export async function saveCarForumArticle(id) {
  return await post(`api/CarForumArticle/${id}/save`);
}

export async function unsaveCarForumArticle(id) {
  return await del(`api/CarForumArticle/${id}/unsave`);
}
