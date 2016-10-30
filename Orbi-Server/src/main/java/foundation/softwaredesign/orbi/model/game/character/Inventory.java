package foundation.softwaredesign.orbi.model.game.character;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectTypeCategory;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.ArrayList;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class Inventory {

    private List<InventoryItem> items = new ArrayList<>();
    private List<GameObjectTypeCategory> categories = new ArrayList<>();
    private Boolean newItemDiscovered = false;

    public List<InventoryItem> getItems() {
        return items;
    }

    public void setItems(List<InventoryItem> items) {
        this.items = items;
    }

    public List<GameObjectTypeCategory> getCategories() {
        return categories;
    }

    public void setCategories(List<GameObjectTypeCategory> categories) {
        this.categories = categories;
    }

    public Boolean getNewItemDiscovered() {
        return newItemDiscovered;
    }

    public void setNewItemDiscovered(Boolean newItemDiscovered) {
        this.newItemDiscovered = newItemDiscovered;
    }
}
