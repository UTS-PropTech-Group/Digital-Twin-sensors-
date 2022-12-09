# Sensor Monitoring Dashboard

> This project is based on [Vue Light Bootstrap Dashboard](http://vuejs.creative-tim.com/vue-light-bootstrap-dashboard)

This dashboard is a vue based interface to visualize sensors data stored in the backend. This source is dependant on the backend source so If you haven't already setup backend please first refer to it's documentation.

## Install Docker
The following installation script is built for Ubuntu16+, for other distributions of linux please search online.
```
sudo apt-get remove docker docker-engine docker.io containerd runc
sudo apt-get -y update
sudo apt-get -y install \
    ca-certificates \
    curl \
    gnupg \
    lsb-release

# Docker official GPG key
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg

# set up the stable repository.
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
  
sudo apt-get -y update
sudo apt-get -y install docker-ce docker-ce-cli containerd.io

# verify docker is installed correctly 
sudo docker run hello-world
```

## Start the project

Build the source and run with Docker.
```
docker build -t sensor-dashboard .
docker run -p 8080:8080 --name sensor-dashboard -d sensor-dashboard
```
Verify the app is installed.
```
docker run -p 8080:8080 --name sensor-dashboard -d sensor-dashboard
```

## Connect 3D Heatmap Model

To connect with the [3D model](https://github.com/smart-construction-group/heatmap-3d) please first make sure that you have it hosted somewhere. Then modify the `<iframe>` tag in `/src/pages/HeatMap3D.vue` to match the hosting URL.

