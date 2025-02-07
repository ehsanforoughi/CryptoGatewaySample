const express = require("express");
const app = express();

require("./src/startup/logging");
require("./src/startup/routes")(app);
//require("./src/startup/db")();
require("./src/startup/config")();
require("./src/startup/validation")();

// const p = Promise.reject(new Error("errorrrrrrrr"));
// p.then(() => console.log("Done"));

const port = process.env.PORT || 3000;
app.listen(port, () => console.log(`Listening on port ${port}...`));
