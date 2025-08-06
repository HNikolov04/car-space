import { useRef } from "react";
import SearchIcon from "../../assets/CarSpaceSearchIconWhiteMode.png";

export default function SearchBar({ value, onChange, onSearch }) {
  const inputRef = useRef(null);

  const handleKeyDown = (e) => {
    if (e.key === "Enter") {
      onSearch();
    }
  };

  return (
    <div className="relative w-full">
      <input
        ref={inputRef}
        type="text"
        value={value}
        onChange={onChange}
        onKeyDown={handleKeyDown}
        placeholder="Search services..."
        className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:ring-blue-300 pr-10"
      />
      <img
        src={SearchIcon}
        alt="Search"
        className="w-10 h-10 absolute right-3 top-1/2 transform -translate-y-1/2 cursor-pointer opacity-70 hover:opacity-100"
        onClick={() => onSearch()}
      />
    </div>
  );
}