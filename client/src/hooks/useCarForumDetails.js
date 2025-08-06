import { useEffect, useState } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import {
  getCarForumArticleById,
  saveCarForumArticle,
  unsaveCarForumArticle,
  deleteCarForumArticle,
} from "../services/api/carForumArticleService";
import {
  getCarForumComments,
  addCarForumComment,
  deleteCarForumComment,
} from "../services/api/carForumCommentService";
import { useAuth } from "../contexts/AuthContext";

export function useCarForumDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user, isAuthenticated } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();

  const page = parseInt(searchParams.get("Page") || "1");
  const pageSize = 5;

  const [post, setPost] = useState(null);
  const [comments, setComments] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [newComment, setNewComment] = useState("");
  const [saved, setSaved] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAll = async () => {
      try {
        const post = await getCarForumArticleById(id);
        const commentRes = await getCarForumComments(id, page, pageSize);
        setPost(post);
        setSaved(post.isSavedByCurrentUser ?? false);
        setComments(commentRes.items || []);
        setTotalPages(commentRes.totalPages || 1);
      } catch (err) {
        console.error("Failed to load post:", err);
        navigate("/car-forum");
      } finally {
        setLoading(false);
      }
    };
    fetchAll();
  }, [id, page]);

  const handleAddComment = async () => {
    if (!newComment.trim()) return;
    if (!isAuthenticated) return navigate("/login");

    try {
      const comment = await addCarForumComment(id, { content: newComment });
      setComments((prev) => [comment, ...prev]);
      setNewComment("");
    } catch (err) {
      console.error("Failed to post comment:", err);
    }
  };

  const handleDeleteComment = async (commentId) => {
    if (!window.confirm("Delete this comment?")) return;

    try {
      await deleteCarForumComment(commentId);
      setComments((prev) => prev.filter((c) => c.id !== commentId));
    } catch (err) {
      console.error("Failed to delete comment:", err);
    }
  };

  const handleDeletePost = async () => {
    if (!window.confirm("Delete this post?")) return;

    try {
      await deleteCarForumArticle(id);
      navigate("/car-forum");
    } catch (err) {
      console.error("Failed to delete post:", err);
    }
  };

  const toggleSave = async () => {
    if (!isAuthenticated) return navigate("/login");

    try {
      if (saved) {
        await unsaveCarForumArticle(id);
      } else {
        await saveCarForumArticle(id);
      }
      setSaved((prev) => !prev);
    } catch (err) {
      console.error("Failed to toggle save:", err);
    }
  };

  const handlePageChange = (newPage) => {
    const newParams = new URLSearchParams(searchParams);
    newParams.set("Page", newPage.toString());
    setSearchParams(newParams);
  };

  return {
    post,
    user,
    isOwner: user?.id === post?.userId,
    comments,
    totalPages,
    page,
    saved,
    loading,
    newComment,
    setNewComment,
    handleAddComment,
    handleDeleteComment,
    handleDeletePost,
    toggleSave,
    handlePageChange,
  };
}
