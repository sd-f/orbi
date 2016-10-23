package foundation.softwaredesign.orbi.model.game.world;

import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.ArrayList;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class World {

    private List<GameObject> gameObjects = new ArrayList<>();
    private List<Character> characters = new ArrayList<>();

    public List<Character> getCharacters() {
        return characters;
    }

    public void setCharacters(List<Character> characters) {
        this.characters = characters;
    }

    public List<GameObject> getGameObjects() {
        return gameObjects;
    }

    public void setGameObjects(List<GameObject> gameObjects) {
        this.gameObjects = gameObjects;
    }

}
