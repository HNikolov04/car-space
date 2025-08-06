import { Link, useNavigate } from "react-router-dom";
import { useCarForumDetails } from "../../hooks/useCarForumDetails";
import CarHeader from "../../components/shared/CarHeader";
import Footer from "../../components/shared/Footer";
import Pagination from "../../components/shared/Pagination";
import SaveIcon from "../../assets/CarSpaceSaveIconLightMode.png";
import SaveIconActive from "../../assets/CarSpaceActiveSaveIconLightMode.png";

export default function CarForumDetailsPage() {
  const {
    post,
    user,
    isOwner,
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
  } = useCarForumDetails();

  const navigate = useNavigate();

  if (loading || !post) {
    return <div className="text-center py-20 text-gray-500">Loading...</div>;
  }

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <CarHeader />

      <main className="flex-1 flex items-center justify-center px-4 py-10">
        <div className="w-full max-w-3xl bg-white rounded-xl shadow-lg border border-gray-300 p-8">
          <div className="flex justify-between items-start mb-4">
            <div>
              <div className="text-sm text-gray-700 font-medium">
                {post.userNickname || "Unknown"}
              </div>
              <div className="text-xs text-gray-500">
                Updated: {new Date(post.updatedAt).toLocaleDateString()}
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

          <h1 className="text-2xl font-bold mb-2">{post.title}</h1>
          <div className="text-sm text-gray-600 italic mb-4">Brand: {post.brandName}</div>

          <div className="border p-4 rounded bg-gray-50 text-gray-800 whitespace-pre-wrap mb-6">
            {post.description}
          </div>

          {user && (
            <div className="mb-6">
              <textarea
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
                placeholder="Add a comment..."
                className="w-full border border-gray-300 rounded-md p-2 mb-3"
              />
              <div className="flex justify-center">
                <button
                  onClick={handleAddComment}
                  className="px-5 py-2 cursor-pointer bg-green-600 hover:bg-green-700 text-white rounded-md shadow text-sm"
                >
                  Post Comment
                </button>
              </div>
            </div>
          )}

          {comments.length > 0 && (
            <div className="space-y-4 mb-6">
              {comments.map((comment) => (
                <div
                  key={comment.id}
                  className="bg-white border border-gray-200 p-4 rounded-lg shadow-sm"
                >
                  <div className="flex justify-between items-center mb-1">
                    <div className="text-sm text-gray-800 font-medium">
                      {comment.userNickname}
                    </div>
                    {comment.userId === user?.id && (
                      <button
                        onClick={() => handleDeleteComment(comment.id)}
                        className="px-3 py-1 cursor-pointer text-xs bg-red-500 text-white rounded-md hover:bg-red-600 shadow-sm"
                      >
                        Delete
                      </button>
                    )}
                  </div>
                  <div className="text-xs text-gray-500 mb-2">
                    Created at: {new Date(comment.createdAt).toLocaleDateString()}
                  </div>
                  <p className="text-sm text-gray-800">{comment.content}</p>
                </div>
              ))}
            </div>
          )}

          {totalPages > 1 && (
            <div className="mb-6">
              <Pagination
                currentPage={page}
                totalPages={totalPages}
                onPageChange={handlePageChange}
              />
            </div>
          )}

          <div className="flex flex-wrap justify-center gap-4">
            {isOwner && (
              <button
                onClick={() => navigate(`/car-forum/edit/${post.id}`)}
                className="px-4 py-2 cursor-pointer bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
              >
                Edit
              </button>
            )}
            <Link
              to="/car-forum"
              className="px-4 py-2 cursor-pointer bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
            >
              Back to Articles
            </Link>
            {isOwner && (
              <button
                onClick={handleDeletePost}
                className="px-4 py-2 cursor-pointer bg-red-500 hover:bg-red-600 text-white rounded-md shadow"
              >
                Delete
              </button>
            )}
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}
