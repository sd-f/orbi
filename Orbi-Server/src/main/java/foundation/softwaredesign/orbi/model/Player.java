package foundation.softwaredesign.orbi.model;

import javax.validation.constraints.NotNull;
import javax.xml.bind.annotation.XmlRootElement;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class Player {
    @NotNull
    private Character character;
    private GameObject gameObjectToCraft;
    private Long selectedObjectId;

    public GameObject getGameObjectToCraft() {
        return gameObjectToCraft;
    }

    public void setGameObjectToCraft(GameObject gameObjectToCraft) {
        this.gameObjectToCraft = gameObjectToCraft;
    }

    public Character getCharacter() {
        return character;
    }

    public void setCharacter(Character character) {
        this.character = character;
    }

    public Long getSelectedObjectId() {
        return selectedObjectId;
    }

    public void setSelectedObjectId(Long selectedObjectId) {
        this.selectedObjectId = selectedObjectId;
    }

}
