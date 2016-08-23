package foundation.softwaredesign.orbi.model;

import javax.xml.bind.annotation.XmlRootElement;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class Player {
    private GeoPosition geoPosition;
    private GameObject gameObjectToCraft;


    public GameObject getGameObjectToCraft() {
        return gameObjectToCraft;
    }

    public void setGameObjectToCraft(GameObject gameObjectToCraft) {
        this.gameObjectToCraft = gameObjectToCraft;
    }

    public GeoPosition getGeoPosition() {
        return geoPosition;
    }

    public void setGeoPosition(GeoPosition geoPosition) {
        this.geoPosition = geoPosition;
    }
}
