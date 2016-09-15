package foundation.softwaredesign.orbi.persistence;

import org.eclipse.persistence.config.SessionCustomizer;
import org.eclipse.persistence.descriptors.ClassDescriptor;
import org.eclipse.persistence.sessions.Session;

import java.sql.SQLException;

/**
 * source: http://iromu.blogspot.co.at/2013/10/eclipselink-and-auto-camelcase-column.html
 * <p>
 *
 * @author Lucas Reeh <lreeh@tugraz.at>
 */
public class CamelCaseSessionCustomizer implements SessionCustomizer {

    @Override
    public void customize(Session session) throws SQLException {
        for (ClassDescriptor descriptor : session.getDescriptors().values()) {
            // Only change the table name for non-embedable entities with no
            // @Table already
            if (!descriptor.getTables().isEmpty() && descriptor.getAlias().equalsIgnoreCase(descriptor.getTableName())) {
                String tableName = addUnderscores(descriptor.getJavaClass().getSimpleName());
                descriptor.setTableName(tableName);
                descriptor.getTables().get(0).getIndexes()
                        .stream()
                        .forEach(
                                index -> index.setTargetTable(tableName)
                        );
            }
            // Only change the column name for non-embeddable entities with no @Column already
            descriptor.getMappings()
                    .stream()
                    .filter(
                            mapping -> mapping.getField() != null
                                    && !mapping.getAttributeName().isEmpty()
                                    && mapping.getField().getName().equalsIgnoreCase(mapping.getAttributeName())
                    )
                    .forEach(
                            mapping -> mapping.getField().setName(addUnderscores(mapping.getAttributeName()))
                    );
        }
    }

    private static String addUnderscores(String name) {
        StringBuilder buf = new StringBuilder(name.replace('.', '_'));
        for (int i = 1; i < buf.length(); i++) {
            int index = i;
            if (Character.isLowerCase(buf.charAt(index - 1)) && Character.isUpperCase(buf.charAt(index))) {
                buf.insert(index++, '_');
            }
        }
        return buf.toString().toUpperCase();
    }

}