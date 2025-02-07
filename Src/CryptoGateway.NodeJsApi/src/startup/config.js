process.env["NODE_CONFIG_DIR"] = "./src/config/";
const config = require("config");
module.exports = function () {
  if (!config.get("jwtPrivateKey")) {
    throw new Error("FATAL ERROR: jwtPrivateKey is not defined.");
  }
};
//setx [tron_jwtPrivateKey] "eyyyyyyyyy"
