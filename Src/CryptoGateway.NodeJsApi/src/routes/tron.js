const auth = require("../middleware/auth");
const express = require("express");
const router = express.Router();
const TronWeb = require("tronweb");
const tronWeb = new TronWeb({
  fullHost: "https://api.trongrid.io",
  headers: { "TRON-PRO-API-KEY": "123" },
  privateKey: "123",
});
// async function createAccount() {
//   const newAccount = await tronWeb.createAccount();
//   console.log(newAccount);
// }

router.get("/getBalance/:address", [auth], async (req, res) => { 
    const tokenContractAddress = "123";
    const contract = await tronWeb.contract().at(tokenContractAddress);
    const balance = await contract.balanceOf(req.params.address).call();
  //const normalizedBalance = tronWeb.toDecimal(balance);//.toNumber();
  res.send({ balance: tronWeb.fromSun(balance.toNumber()) });
});

router.post("/createAccount", [auth], async (req, res) => {
  const newAccount = await tronWeb.createAccount();
  res.send(newAccount);
});

router.get("/getAccount/:address", [auth], async (req, res) => {
  await tronWeb.trx.getAccount(req.params.address)
    .then((result) => res.send({ account: result}));
});

router.get("/getTransaction/:txId", [auth], async (req, res) => { 
  tronWeb.trx.getTransaction(req.params.txId)
    .then((result) => res.send({ account: result }));
});

router.get("/getTransactions/:address", [auth], async (req, res) => {
  const axios = require('axios');

  const options = {
    method: 'GET',
    url: `https://api.trongrid.io/v1/accounts/${req.params.address}/transactions`,
    headers: {accept: 'application/json'}
  };
  
  axios
    .request(options)
    .then((response) => {
      res.send({ result: response.data })
    });
});

router.get("/getTrc20Transactions/:address", [auth], async (req, res) => {
  const axios = require('axios');

  const options = {
    method: 'GET',
    url: `https://api.trongrid.io/v1/accounts/${req.params.address}/transactions/trc20`,
    headers: {accept: 'application/json'}
  };
  
  axios
    .request(options)
    .then((response) => {
      res.send({ result: response.data })
    });
});

router.post("/sendTrx", [auth], async (req, res) => {
  const privateKey = "123"; 
  var fromAddress = req.body.fromAddress; //address _from
  var toAddress = req.body.toAddress; //address _to
  var amount = tronWeb.toSun(req.body.amount); //amount
  //Creates an unsigned TRX transfer transaction
  tradeobj = await tronWeb.transactionBuilder.sendTrx(
        toAddress,
        amount,
        fromAddress
  );
  const signedtxn = await tronWeb.trx.sign(
        tradeobj,
        privateKey
  );
  const receipt = await tronWeb.trx.sendRawTransaction(
    signedtxn
  ).then(output => { res.send({ Output: output }); });
});
//-------------------------------------------------------------------------------------
router.post("/sendTether", [auth], async (req, res) => {
  const privateKey = req.body.privateKey; 
  var fromAddress = req.body.fromAddress; //address _from
  var toAddress = req.body.toAddress; //address _to
  var amount = tronWeb.toSun(req.body.amount); //amount
  //Creates an unsigned TRX transfer transaction
  const options = {
    feeLimit: 1000000000,
    callValue: 0
  };
  
  const tx = await tronWeb.transactionBuilder.triggerSmartContract(
    "123", 'transfer(address,uint256)', options,
    [{
      type: 'address',
      value: toAddress
    }, {
      type: 'uint256',
      value: amount
    }],
    tronWeb.address.toHex(fromAddress)
  );

  const signedTx = await tronWeb.trx.sign(tx.transaction, privateKey);
  const broadcastTx = await tronWeb.trx.sendRawTransaction(signedTx)
    .then(output => { res.send({ Output: output }); });

  console.log(signedTx);
  console.log(broadcastTx);
});
//-------------------------------------------------------------------------------------
router.get("/getContractInfo/:address", [auth], async (req, res) => { 
  const axios = require('axios');

  const options = {
    method: 'POST',
    url: 'https://api.trongrid.io/wallet/getcontractinfo',
    headers: {accept: 'application/json', 'content-type': 'application/json'},
    data: {value: req.params.address, visible: true}
  };
  
  axios
    .request(options)
    .then((response) => {
      res.send({ result: response.data })
    });
});

router.get("/getContract/:address", [auth], async (req, res) => { 
  const axios = require('axios');

const options = {
  method: 'POST',
  url: 'https://api.trongrid.io/wallet/getcontract',
  headers: {accept: 'application/json', 'content-type': 'application/json'},
  data: {value: req.params.address, visible: true}
};

axios
  .request(options)
  .then((response) => {
    res.send({ result: response.data })
  });
});


router.get("/getAddress", async (req, res) => {
  const address = tronWeb.address.fromPrivateKey("123");
  res.send({ result: address });
});

module.exports = router;
