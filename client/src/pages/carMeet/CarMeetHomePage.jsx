import { useSearchParams } from "react-router-dom";
import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import Pagination from "../../components/shared/Pagination";
import useCarMeets from "../../hooks/useCarMeets";

export default function CarMeetHomePage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const {
    meets,
    totalPages,
    loading,
    error,
    page,
    selectedDate,
    handleFilterModeChange,
    handlePageChange,
    updateParams,
    filterMode,
  } = useCarMeets(searchParams, setSearchParams);

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <CarHeader
        searchTerm={searchParams.get("SearchTerm") || ""}
        setSearchTerm={(val) => updateParams({ SearchTerm: val, Page: "1" })}
      />

      <main className="flex-1 flex p-4">
        <aside className="w-64 mr-4">
          <FilterSidebar
            selectedDate={selectedDate}
            setSelectedDate={(val) => updateParams({ FilterByDate: val, Page: "1" })}
            filterMode={filterMode}
            setFilterMode={handleFilterModeChange}
          />
        </aside>

        <section className="flex-1 flex flex-col">
          {loading ? (
            <div className="flex-1 flex items-center justify-center text-gray-500">
              Loading car meets...
            </div>
          ) : error ? (
            <div className="flex-1 flex items-center justify-center text-red-500">
              {error}
            </div>
          ) : meets.length === 0 ? (
            <div className="flex-1 flex items-center justify-center text-gray-500">
              No car meets found.
            </div>
          ) : (
            <>
              <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                {meets.map((meet) => (
                  <ListingCard key={meet.id} meet={meet} />
                ))}
              </div>
              <div className="mt-auto pt-6">
                <Pagination
                  currentPage={page}
                  totalPages={totalPages}
                  onPageChange={handlePageChange}
                />
              </div>
            </>
          )}
        </section>
      </main>

      <Footer />
    </div>
  );
}

import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../utils/routes/routes";
import { useAuth } from "../../contexts/AuthContext";

function FilterSidebar({ selectedDate, setSelectedDate, filterMode, setFilterMode }) {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const tab = (label, active, onClick) => (
    <div
      onClick={onClick}
      className={`text-sm font-semibold px-4 py-2 rounded-md text-center border border-gray-300 transition ${
        active ? "bg-blue-600 text-white" : "bg-white text-black hover:bg-blue-100 cursor-pointer"
      }`}
    >
      {label}
    </div>
  );

  const handleCreate = () => {
    navigate(isAuthenticated ? ROUTES.carMeetCreate : ROUTES.login);
  };

  const handleProtected = (mode) => {
    isAuthenticated ? setFilterMode(mode) : navigate(ROUTES.login);
  };

  const handleClear = () => {
    setSelectedDate("");
  };

  return (
    <aside className="w-full h-full pr-4 border-r border-gray-300 text-sm space-y-4">
      <button
        onClick={handleCreate}
        className="w-full text-center bg-white border border-blue-600 text-blue-600 rounded-md py-2 font-semibold hover:bg-blue-600 hover:text-white transition"
      >
        + Create Meet
      </button>

      <div className="space-y-1">
        {tab("All Meets", filterMode === "all", () => setFilterMode("all"))}
        {tab("My Meets", filterMode === "mine", () => handleProtected("mine"))}
        {tab("Joined Meets", filterMode === "joined", () => handleProtected("joined"))}
        {tab("Saved Meets", filterMode === "saved", () => handleProtected("saved"))}
      </div>

      <div className="pt-4 space-y-2">
        {tab("Clear All Filters", false, handleClear)}

        <input
          type="date"
          value={selectedDate}
          onChange={(e) => setSelectedDate(e.target.value)}
          className="w-full border bg-white border-gray-300 rounded-md px-3 py-1 focus:outline-none focus:ring focus:ring-blue-200 appearance-none"
        />
      </div>
    </aside>
  );
}

import { useState } from "react";
import { Link } from "react-router-dom";
import { saveCarMeetListing, unsaveCarMeetListing } from "../../services/api/carMeetListingService";
import SaveIcon from "../../assets/CarSpaceSaveIconLightMode.png";
import SaveIconActive from "../../assets/CarSpaceActiveSaveIconLightMode.png";

function ListingCard({ meet }) {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [isSaved, setIsSaved] = useState(meet.isSavedByCurrentUser || false);

  const handleToggleSave = async (e) => {
    e.preventDefault();
    if (!isAuthenticated) return navigate("/login");

    try {
      isSaved
        ? await unsaveCarMeetListing(meet.id)
        : await saveCarMeetListing(meet.id);
      setIsSaved((prev) => !prev);
    } catch (err) {
      console.error("Save toggle failed:", err);
    }
  };

  const shortDescription =
    meet.description.split(" ").slice(0, 15).join(" ") +
    (meet.description.split(" ").length > 15 ? "..." : "");

  const imageUrl = meet.imageUrl
    ? `${import.meta.env.VITE_API_BASE_URL}${meet.imageUrl}`
    : "https://via.placeholder.com/300x200";

  return (
    <Link
      to={`/car-meet/${meet.id}`}
      className="bg-white border border-gray-300 rounded-lg hover:shadow-md transition p-3 flex flex-col justify-between h-[380px] relative"
    >
      <div className="flex justify-between items-center text-xs text-gray-600 font-medium">
        <span>{meet.userNickname || "Unknown"}</span>
        <img
          src={isSaved ? SaveIconActive : SaveIcon}
          alt="Save"
          onClick={handleToggleSave}
          className="w-8 h-8 cursor-pointer"
          title={isSaved ? "Unsave" : "Save"}
        />
      </div>
      <div className="text-xs text-gray-500 mb-2">
        Updated: {new Date(meet.updatedAt).toLocaleDateString()}
      </div>
      <div className="w-full h-36 bg-gray-200 rounded-md overflow-hidden mb-2">
        <img src={imageUrl} alt={meet.title} className="w-full h-full object-cover" />
      </div>
      <div className="text-sm text-center mb-2">
        <h3 className="font-semibold text-base">{meet.title}</h3>
        <p className="text-gray-600 text-sm mt-1">{shortDescription}</p>
      </div>
      <div className="flex justify-between text-xs text-gray-500 mt-auto">
        <span>{meet.city}</span>
        <span>Meet Date: {new Date(meet.meetDate).toLocaleDateString()}</span>
      </div>
    </Link>
  );
}
