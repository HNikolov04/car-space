import { useEffect, useState } from "react";
import { useAuth } from "../contexts/AuthContext";
import { getCarMeets } from "../services/api/carMeetListingService";

export default function useCarMeets(searchParams, setSearchParams) {
  const { isAuthenticated } = useAuth();
  const [meets, setMeets] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const page = parseInt(searchParams.get("Page") || "1");
  const pageSize = parseInt(searchParams.get("PageSize") || "8");
  const searchTerm = searchParams.get("SearchTerm") || "";
  const selectedDate = searchParams.get("FilterByDate") || "";
  const savedOnly = searchParams.get("SavedOnly") === "true";
  const myMeetsOnly = searchParams.get("MyMeetsOnly") === "true";
  const joinedOnly = searchParams.get("JoinedOnly") === "true";

  const filterMode = savedOnly
    ? "saved"
    : myMeetsOnly
    ? "mine"
    : joinedOnly
    ? "joined"
    : "all";

  const updateParams = (params) => {
    const newParams = new URLSearchParams(searchParams);
    Object.entries(params).forEach(([key, val]) => {
      if (val === null || val === undefined || val === "") {
        newParams.delete(key);
      } else {
        newParams.set(key, val);
      }
    });
    setSearchParams(newParams);
  };

  const handlePageChange = (newPage) => {
    updateParams({ Page: newPage.toString() });
  };

  const handleFilterModeChange = (mode) => {
    updateParams({
      SavedOnly: mode === "saved" ? "true" : null,
      MyMeetsOnly: mode === "mine" ? "true" : null,
      JoinedOnly: mode === "joined" ? "true" : null,
      Page: "1",
    });
  };

  useEffect(() => {
    const fetchMeets = async () => {
      setLoading(true);
      setError("");

      try {
        const data = await getCarMeets({
          Page: page,
          PageSize: pageSize,
          SearchTerm: searchTerm || null,
          FilterByDate: selectedDate || null,
          SavedOnly: isAuthenticated && savedOnly,
          MyMeetsOnly: isAuthenticated && myMeetsOnly,
          JoinedOnly: isAuthenticated && joinedOnly,
        });

        setMeets(data.items || []);
        setTotalPages(data.totalPages || 1);
      } catch (err) {
        console.error("Failed to fetch car meets:", err);
        setError("Failed to load car meets. Please try again.");
      } finally {
        setLoading(false);
      }
    };

    fetchMeets();
  }, [
    page,
    pageSize,
    searchTerm,
    selectedDate,
    savedOnly,
    myMeetsOnly,
    joinedOnly,
    isAuthenticated,
  ]);

  return {
    meets,
    totalPages,
    loading,
    error,
    page,
    selectedDate,
    filterMode,
    updateParams,
    handlePageChange,
    handleFilterModeChange,
  };
}
