curl -X 'POST' \
  'https://localhost:7033/orders' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d @create-order-body.json