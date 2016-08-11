package foundation.softwaredesign.orbi.model.virtual;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class World {

    private List<Cube> cubes;

    public List<Cube> getCubes() {
        return cubes;
    }

    public void setCubes(List<Cube> cubes) {
        this.cubes = cubes;
    }
}
