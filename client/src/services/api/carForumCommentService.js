import { httpClient } from "./httpClient";

export async function getCarForumComments(articleId, page = 1, pageSize = 10) {
  return await httpClient(`api/CarForumComment/${articleId}/comments?page=${page}&pageSize=${pageSize}`);
}

export async function addCarForumComment(articleId, data) {
  return await httpClient(`api/CarForumComment/${articleId}/comments`, {
    method: "POST",
    data,
  });
}

export async function deleteCarForumComment(commentId) {
  return await httpClient(`api/CarForumComment/comments/${commentId}`, {
    method: "DELETE",
  });
}
