docker run --tty --rm \
    --network sqlserver-outbox_default \
    quay.io/debezium/tooling \
    kafkacat -b kafka:29092 -C -o beginning -q \
    -f "{\"key\":%k, \"headers\":\"%h\"}\n%s\n" \
    -t Order.events | jq .