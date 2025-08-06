import { Link, useParams, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import SaveIcon from "../../assets/CarSpaceSaveIconLightMode.png";
import SaveIconActive from "../../assets/CarSpaceActiveSaveIconLightMode.png";
import useCarMeetDetails from "../../hooks/useCarMeetDetails";

export default function CarMeetDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const {
    meet,
    participants,
    loading,
    isOwner,
    isLoggedIn,
    saved,
    joined,
    toggleSave,
    handleJoin,
    handleLeave,
    handleDelete,
  } = useCarMeetDetails(id, navigate);

  if (loading || !meet) return <div className="text-center py-10">Loading...</div>;

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <CarHeader />

      <main className="flex-1 p-6 flex flex-col items-center">
        <div className="w-full max-w-6xl flex flex-col lg:flex-row gap-6">
          <div className="flex-1 bg-white shadow-md border border-gray-300 rounded-xl p-6 relative">
            <div className="flex justify-between items-start text-sm text-gray-600 mb-4">
              <div>
                <div className="font-medium mb-1">{meet.userNickname || "Unknown"}</div>
                <div className="text-gray-500 text-xs">
                  Updated on {new Date(meet.updatedAt).toLocaleDateString()}
                </div>
              </div>
              <img
                src={saved ? SaveIconActive : SaveIcon}
                alt="Save"
                className="w-10 h-10 cursor-pointer transition-transform hover:scale-110"
                onClick={toggleSave}
                title={saved ? "Unsave" : "Save"}
              />
            </div>

            <div className="w-full h-64 bg-gray-200 rounded-md overflow-hidden mb-4">
              <img
                src={
                  meet.imageUrl
                    ? `${import.meta.env.VITE_API_BASE_URL}${meet.imageUrl}`
                    : "https://via.placeholder.com/600x300"
                }
                alt={meet.title}
                className="w-full h-full object-cover"
              />
            </div>

            <h2 className="text-2xl font-bold text-gray-800 mb-2">{meet.title}</h2>
            <p className="text-gray-600 mb-4">{meet.description}</p>

            <div className="flex justify-between text-sm text-gray-700 font-medium mb-6">
              <p><span className="font-semibold">Location:</span> {meet.city}, {meet.address}</p>
              <p><span className="font-semibold">Meet Date:</span> {new Date(meet.meetDate).toLocaleDateString()}</p>
            </div>

            <div className="mt-8 flex justify-center gap-4 flex-wrap items-center">
              {isOwner ? (
                <>
                  <Link to={`/car-meet/edit/${meet.id}`} className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow">
                    Edit
                  </Link>
                  <Link to="/car-meet" className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow">
                    Back to CarMeets
                  </Link>
                  <button onClick={handleDelete} className="px-4 py-2 bg-red-500 hover:bg-red-600 text-white rounded-md shadow">
                    Delete
                  </button>
                </>
              ) : (
                <>
                  {isLoggedIn && joined && (
                    <button onClick={handleLeave} className="px-4 py-2 bg-red-500 hover:bg-red-600 text-white rounded-md shadow">
                      Leave Meet
                    </button>
                  )}
                  <Link to="/car-meet" className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow">
                    Back to CarMeets
                  </Link>
                  {isLoggedIn && !joined && (
                    <button onClick={handleJoin} className="px-4 py-2 bg-green-600 hover:bg-green-700 text-white rounded-md shadow">
                      Join Meet
                    </button>
                  )}
                </>
              )}
            </div>
          </div>

          <aside className="w-full lg:w-64 bg-white shadow-md border border-gray-300 rounded-xl p-6 h-fit">
            <h3 className="text-center font-bold text-sm mb-3 tracking-wide">PARTICIPANTS</h3>
            <ul className="text-sm text-gray-700 space-y-1">
              {participants.length === 0 ? (
                <li className="text-center italic text-gray-400">No participants yet</li>
              ) : (
                participants.map((p) => (
                  <li key={p.userId}>• {p.username || "Unnamed User"}</li>
                ))
              )}
            </ul>
          </aside>
        </div>
      </main>

      <Footer />
    </div>
  );
}
