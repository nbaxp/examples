# Docker

## 安装

### [Ubuntu 22.04](https://docs.docker.com/engine/install/ubuntu/) 2024/4/3

1. 卸载可能冲突的包:

   ```sh
   for pkg in docker.io docker-doc docker-compose docker-compose-v2 podman-docker containerd runc; do sudo apt-get remove $pkg; done
   ```

1. 卸载老版本

   ```sh
   sudo apt-get purge docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin docker-ce-rootless-extras
   sudo rm -rf /var/lib/docker
   sudo rm -rf /var/lib/containerd
   ```

1. Set up Docker's apt repository.

   ```sh
   # Add Docker's official GPG key:
   sudo apt-get update
   sudo apt-get install ca-certificates curl
   sudo install -m 0755 -d /etc/apt/keyrings
   sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
   sudo chmod a+r /etc/apt/keyrings/docker.asc

   # Add the repository to Apt sources:
   echo \
   "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
   $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
   sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
   sudo apt-get update
   ```

1. Install the Docker packages.

   ```sh
   sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
   ```

1. 使用命令 `docker info` 或 `sudo docker run hello-world` 验证安装

## 配置

1. 编辑 `/lib/systemd/system/docker.service` 在 [Service] 节点下找到 ExecStart 配置项，追加 `--insecure-registry=host.docker.internal:8082` 参数设置 http 镜像仓库
