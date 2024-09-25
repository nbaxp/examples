FROM openjdk:8

ARG VERSION
# Build from Source Code And Copy it into image
# COPY ./target/apache-seatunnel-${VERSION}-bin.tar.gz /opt/

# Download From Internet
# Please Note this file only include fake/console connector, You'll need to download the other connectors manually
RUN wget -P /opt https://dlcdn.apache.org/seatunnel/${VERSION}/apache-seatunnel-${VERSION}-bin.tar.gz && \
    cd /opt && \
    tar -zxvf apache-seatunnel-${VERSION}-bin.tar.gz && \
    mv apache-seatunnel-${VERSION} seatunnel && \
    rm apache-seatunnel-${VERSION}-bin.tar.gz && \
    cp seatunnel/config/log4j2_client.properties seatunnel/config/log4j2.properties && \
    cp seatunnel/config/hazelcast-master.yaml seatunnel/config/hazelcast-worker.yaml

WORKDIR /opt/seatunnel