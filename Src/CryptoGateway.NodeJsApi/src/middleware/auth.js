const jwt = require("jsonwebtoken");
process.env["NODE_CONFIG_DIR"] = "./src/config/";
const config = require("config");
const secretKey = "123";

module.exports = function (req, res, next) {
  //const token = req.header("x-auth-token");
  //if (!token) return res.status(401).send("Access denied. No token provided.");
  const secretKeyValue = req.headers['secret-key'];
  if (secretKeyValue !== secretKey) {
    res.status(401).send("Unauthorized Error.");
    return;
  }

  // try {
  //   const decoded = jwt.verify(token, config.get("jwtPrivateKey"));
  //   req.user = decoded;
     next();
  // } catch (ex) {
  //   res.status(400).send("Invalid token.");
  // }
};
