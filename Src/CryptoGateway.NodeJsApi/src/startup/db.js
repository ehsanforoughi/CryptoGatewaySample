//const logger = require("./logging");
const { Sequelize } = require("sequelize");

module.exports = async function () {
  const sequelize = new Sequelize("TronDb", "sa", "123", {
    host: "localhost",
    dialect: "mssql",
    logging: false,
  });

  try {
    await sequelize.authenticate();
    console.log("Connection has been established successfully.");
  } catch (error) {
    console.error("Unable to connect to the database:", error);
  }
};
