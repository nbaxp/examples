FROM docker:26.0.0-dind

RUN \
    apk update && \
    apk add bash

RUN \
    apk add openssh && \
    apk add openrc --no-cache && \
    rc-update add sshd boot && \
    rc-status && \
    touch /run/openrc/softlevel

WORKDIR /

EXPOSE 22

RUN echo 'root:root' | chpasswd

RUN sed -i 's/#PermitRootLogin prohibit-password/PermitRootLogin yes/' /etc/ssh/sshd_config

