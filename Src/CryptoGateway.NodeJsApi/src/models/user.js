const Joi = require("joi");
const { Sequelize, DataTypes } = require("sequelize");

// Override timezone formatting for MSSQL
Sequelize.DATE.prototype._stringify = function _stringify(date, options) {
  return this._applyTimezone(date, options).format("YYYY-MM-DD HH:mm:ss.SSS");
};

const sequelize = new Sequelize("TronDb", "sa", "123", {
  host: "localhost",
  dialect: "mssql",
});
const userSchema = sequelize.define(
  "User",
  {
    id: {
      type: DataTypes.INTEGER,
      autoIncrement: true,
      primaryKey: true,
    },
    name: {
      type: DataTypes.STRING,
      allowNull: false,
      validate: {
        min: 5,
        max: 50,
      },
    },
    email: {
      type: DataTypes.STRING,
      allowNull: false,
      unique: true,
      validate: {
        min: 5,
        max: 255,
      },
    },
    password: {
      type: DataTypes.STRING,
      allowNull: false,
      validate: {
        min: 5,
        max: 255,
      },
    },
    salt: {
      type: DataTypes.STRING,
      allowNull: false,
      validate: {
        min: 5,
        max: 255,
      },
    },
    createdAt: {
      type: DataTypes.DATE,
      allowNull: false,
    },
    updatedAt: {
      type: DataTypes.DATE,
      allowNull: false,
    },

    //isAdmin: { type: DataTypes.BOOLEAN },
  },
  { tableName: "User", timestamps: true }
);

//console.log(userSchema === sequelize.models.User); // true

function validateUser(user) {
  const schema = Joi.object({
    name: Joi.string().min(5).max(50).required(),
    email: Joi.string().min(5).max(255).required().email(),
    password: Joi.string().min(5).max(255).required(),
  });

  return schema.validate(user);
}

exports.User = userSchema;
exports.validate = validateUser;
