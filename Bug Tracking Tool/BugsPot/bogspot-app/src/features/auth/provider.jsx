import { useState, createContext } from "react";
import { fetchApi } from "../../modules/api";

export const AuthContext = createContext(undefined);

export function AuthProvider({ children }) {
  const [userInfo, setUserInfo] = useState(undefined);
  const [fullName, setFullName] = useState(undefined);
  const [tempVariable, setTempVariable] = useState(undefined);

  async function getUser() {
    console.log("inside getUser");

    const json = await fetchApi({
      path: "Auth/User",
    });

    setUserInfo(json);
    setFullName(json.fullName);
  }

  async function getTempVariable(val) {
    setTempVariable(val);
  }

  console.log(JSON.stringify(userInfo));

  return (
    <AuthContext.Provider
      value={{
        userInfo,
        setUserInfo,
        fullName,
        setFullName,
        getUser,
        tempVariable,
        getTempVariable,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
