import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { getCarForumArticles } from "../services/api/carForumArticleService";
import { useAuth } from "../contexts/AuthContext";

export function useCarForumHome() {
  const { isAuthenticated } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();

  const page = parseInt(searchParams.get("Page") || "1");
  const pageSize = parseInt(searchParams.get("PageSize") || "4");
  const searchTerm = searchParams.get("SearchTerm") || "";
  const savedOnly = searchParams.get("SavedOnly") === "true";
  const myArticlesOnly = searchParams.get("MyArticlesOnly") === "true";
  const brandId = searchParams.get("BrandId")
    ? parseInt(searchParams.get("BrandId"))
    : null;

  const [posts, setPosts] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const updateParams = (params) => {
    const newParams = new URLSearchParams(searchParams.toString());
    Object.entries(params).forEach(([key, val]) => {
      if (val === null || val === undefined || val === "") {
        newParams.delete(key);
      } else {
        newParams.set(key, val);
      }
    });
    setSearchParams(newParams);
  };

  const setSearchTerm = (term) => updateParams({ SearchTerm: term, Page: "1" });
  const setBrandId = (id) => updateParams({ BrandId: id ?? "", Page: "1" });

  const handleFilterModeChange = (mode) => {
    updateParams({
      SavedOnly: mode === "saved" ? "true" : null,
      MyArticlesOnly: mode === "mine" ? "true" : null,
      Page: "1",
    });
  };

  const handlePageChange = (newPage) => {
    updateParams({ Page: newPage.toString() });
  };

  const filterMode = savedOnly ? "saved" : myArticlesOnly ? "mine" : "all";

  useEffect(() => {
    const fetchPosts = async () => {
      setLoading(true);
      setError("");

      try {
        const data = await getCarForumArticles({
          Page: page,
          PageSize: pageSize,
          SearchTerm: searchTerm || null,
          SavedOnly: isAuthenticated && savedOnly,
          MyArticlesOnly: isAuthenticated && myArticlesOnly,
          BrandId: brandId || null,
        });

        setPosts(data.items || []);
        setTotalPages(data.totalPages || 1);
      } catch (err) {
        console.error("Failed to load forum posts:", err);
        setError("Failed to load forum posts.");
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, [
    page,
    pageSize,
    searchTerm,
    savedOnly,
    myArticlesOnly,
    brandId,
    isAuthenticated,
  ]);

  return {
    posts,
    totalPages,
    page,
    searchTerm,
    brandId,
    filterMode,
    setSearchTerm,
    setBrandId,
    handleFilterModeChange,
    handlePageChange,
    loading,
    error,
  };
}
